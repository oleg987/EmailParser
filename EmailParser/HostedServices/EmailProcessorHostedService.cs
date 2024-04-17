using EmailParser.DataStorage;
using EmailParser.Services;

namespace EmailParser.HostedServices;

public class EmailProcessorHostedService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly PeriodicTimer _timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

    public EmailProcessorHostedService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Process(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task Process(CancellationToken cancellationToken)
    {
        while (await _timer.WaitForNextTickAsync(cancellationToken) && !cancellationToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var reader = scope.ServiceProvider.GetRequiredService<IEmailReaderService>();
            var storage = scope.ServiceProvider.GetRequiredService<IDataStorage>();

            var requests = await reader.GetNewRequestsAsync(cancellationToken);

            if (requests.Count > 0)
            {
                await storage.StoreAsync(requests, cancellationToken);
            }
        }
    }
}