using Microsoft.EntityFrameworkCore;
using ParallelQueries.CoreApp.Infrastructure.Seeds;

namespace ParallelQueries.CoreApp.Infrastructure;

public class CoreDbInitializer(CoreDbContext coreDbContext)
{
    public async Task Seed()
    {
        await coreDbContext.Database.EnsureCreatedAsync();
        await SeedCustomers(coreDbContext);
    }
    private static async Task SeedCustomers(CoreDbContext coreDbContext)
    {
        if (!await coreDbContext.Customers.AnyAsync())
        {
            var customers = CustomerSeed.GetCustomers();
            await coreDbContext.Customers.AddRangeAsync(customers);
            await coreDbContext.SaveChangesAsync();
        }
    }
}
