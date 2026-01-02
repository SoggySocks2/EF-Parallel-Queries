using ParallelQueries.CoreApp.Models.Customers;

namespace ParallelQueries.CoreApp.Api.Customers;

public interface ICustomerService
{
    Task<CustomerBreakdownByAgeResponse> GetCustomerBreakdownAsync(QueryMode queryMode, CancellationToken cancellation);
}
