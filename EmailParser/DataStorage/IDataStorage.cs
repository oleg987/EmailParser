using EmailParser.Entities;

namespace EmailParser.DataStorage;

public interface IDataStorage
{
    Task StoreAsync(List<CustomerRequest> requests, CancellationToken cancellationToken);
}