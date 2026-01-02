using Microsoft.EntityFrameworkCore;
using ParallelQueries.CoreApp.Infrastructure.Seeds;

namespace ParallelQueries.CoreApp.Infrastructure;

public static class CoreDbInitializer
{
    public static async Task Seed()
    {
        var coreDbContext = CoreDbContextBuilder.Build();
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
