using Microsoft.EntityFrameworkCore;
using ParallelQueries.CoreApp.Domain.Customers;

namespace ParallelQueries.CoreApp.Infrastructure.Customers;

public class ParallelCustomerQuery(IDbContextFactory<CoreDbContext> contextFactory) : IParallelCustomerQuery
{
    public async Task<IQueryable<Customer>> GetCustomerByMaxCreatedAsync(DateTime maxCreated)
    {
        var coreDbContext = await contextFactory.CreateDbContextAsync();
        return coreDbContext.Customers
            .Where(c => c.Created <= maxCreated);
    }

    public async Task<IQueryable<Customer>> GetCustomerByMaxCreatedAndGenderAsync(DateTime maxCreated, string gender)
    {
        var qry = await GetCustomerByMaxCreatedAsync(maxCreated);
        return qry
            .AsNoTracking()
            .Where(c => c.Person.Gender.Equals(gender));
    }

    public async Task<IQueryable<Customer>> GetCustomerByMaxCreatedBetweenMinAndMaxAgeAsync(DateTime maxCreated, int minAge, int maxAge)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var qry = await GetCustomerByMaxCreatedAsync(maxCreated);
        return qry
            .AsNoTracking()
            .Where(c => c.Dob.HasValue && c.Dob.Value >= today.AddYears(0 - maxAge))
            .Where(c => c.Dob.HasValue && c.Dob.Value <= today.AddYears(0 - minAge));
    }
}