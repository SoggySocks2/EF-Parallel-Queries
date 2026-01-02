using ParallelQueries.CoreApp.Domain.Customers;

namespace ParallelQueries.CoreApp.Infrastructure.Customers;

public class SeriesCustomerQuery(CoreDbContext coreDbContext) : ISeriesCustomerQuery
{
    public async Task<IQueryable<Customer>> GetCustomerByMaxCreatedAsync(DateTime maxCreated)
    {
        return await Task.Run(() => coreDbContext.Customers
            .Where(c => c.Created <= maxCreated));
    }

    public async Task<IQueryable<Customer>> GetCustomerByMaxCreatedAndGenderAsync(DateTime maxCreated, string gender)
    {
        var qry = await GetCustomerByMaxCreatedAsync(maxCreated);
        return qry
            .Where(c => c.Person.Gender.Equals(gender));
    }

    public async Task<IQueryable<Customer>> GetCustomerByMaxCreatedBetweenMinAndMaxAgeAsync(DateTime maxCreated, int minAge, int maxAge)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var qry = await GetCustomerByMaxCreatedAsync(maxCreated);
        return qry
            .Where(c => c.Dob.HasValue && c.Dob.Value >= today.AddYears(0 - maxAge))
            .Where(c => c.Dob.HasValue && c.Dob.Value <= today.AddYears(0 - minAge));
    }
}
