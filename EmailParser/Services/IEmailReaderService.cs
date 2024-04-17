using EmailParser.Entities;

namespace EmailParser.Services;

public interface IEmailReaderService
{
    Task<List<CustomerRequest>> GetNewRequestsAsync(CancellationToken cancellationToken = default);
}