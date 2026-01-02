using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ParallelQueries.SharedKernel.Configuration;

namespace ParallelQueries.CoreApp.Infrastructure.Setup;

public static class CoreAppInfrastructureServices
{
    public static async Task AddCoreAppInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContextFactory<CoreDbContext>(options =>
        {
            options.UseSqlServer(CoreAppInfrastructureConfig.Instance.ConnectionString);
        });

        using var scope = services.BuildServiceProvider().CreateScope();
        var coreDbInitializer = new CoreDbInitializer(scope.ServiceProvider.GetRequiredService<CoreDbContext>());
        await coreDbInitializer.Seed();
    }
}