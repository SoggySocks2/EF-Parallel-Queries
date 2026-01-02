using ParallelQueries.CoreApp.Domain.Customers;

namespace ParallelQueries.CoreApp.Infrastructure.Customers;

public interface ICustomerQuery
{
    Task<IQueryable<Customer>> GetCustomerByMaxCreatedAsync(DateTime maxCreated);
    Task<IQueryable<Customer>> GetCustomerByMaxCreatedAndGenderAsync(DateTime maxCreated, string gender);
    Task<IQueryable<Customer>> GetCustomerByMaxCreatedBetweenMinAndMaxAgeAsync(DateTime maxCreated, int minAge, int maxAge);
}
