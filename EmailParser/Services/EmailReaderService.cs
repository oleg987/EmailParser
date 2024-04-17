using EAGetMail;
using EmailParser.Entities;
using EmailParser.Parsers;

namespace EmailParser.Services;

/// <summary>
/// Service for reading new incoming e-mails.
/// </summary>
public class EmailReaderService : IDisposable, IEmailReaderService
{
    private readonly MailServer _server;
    private readonly MailClient _client;
    private readonly ILogger<EmailReaderService> _logger;

    public EmailReaderService(ILogger<EmailReaderService> logger)
    {
        _logger = logger;
        
        // Configure mail server.
        _server = new MailServer("{mail_host}", "{login}", "{password}", ServerProtocol.Imap4);
        _server.SSLConnection = true;
        
        // Configure mail client.
        _client = new MailClient("TryIt");
    }

    public async Task<List<CustomerRequest>> GetNewRequestsAsync(CancellationToken cancellationToken = default)
    {
        var requests = new List<CustomerRequest>();
        
        await _client.ConnectAsync(_server);

        if (!_client.Connected)
        {
            _logger.LogError($"{DateTime.Now}: Failed connect to mail sever.");
            return [];
        }
        
        // Get inbox folder.
        var folder = (await _client.GetFoldersAsync()).First(f => f.Name == "INBOX");
        
        // Move to inbox folder.
        await _client.SelectFolderAsync(folder);
        
        // Filter only unread e-mails.
        _client.GetMailInfosParam.GetMailInfosOptions = GetMailInfosOptionType.NewOnly;
        
        // TODO: Take allowed senders from config.
        requests.AddRange(await ReadMails("info@it-school.com.ua"));
        requests.AddRange(await ReadMails("admin@abiturients.info"));
        
        return requests;
    }

    private async Task<List<CustomerRequest>> ReadMails(string sender)
    {
        var requests = new List<CustomerRequest>();
        
        // Filter by sender. 
        _client.GetMailInfosParam.SenderContains = sender;
        
        // Get e-mail descriptors.
        var infos = await _client.GetMailInfosAsync();
        
        foreach (var info in infos)
        {
            // Read e-mail message. 
            var mail = await _client.GetMailAsync(info);

            // Get message body.
            var body = mail.TextBody;

            // Parse mail.
            CustomerRequest? request = ParseMessage(sender, body);

            if (request is not null)
            {
                requests.Add(request);
            }

            // Mark e-mail message as read.
            await _client.MarkAsReadAsync(info, true);
        }

        return requests;
    }

    private CustomerRequest? ParseMessage(string sender, string body)
    {
        return CustomerRequestParser.Parse(sender, body);
    }

    private bool ValidatePhone(CustomerRequest request)
    {
        // TODO: Add phone validation. Coming soon...
        return true;
    }

    public void Dispose()
    {
        // Close client, on object dispose.
        _client.Close();
    }
}