using Microsoft.Extensions.DependencyInjection;
using ParallelQueries.CoreApp.Api.Customers;
using ParallelQueries.CoreApp.Infrastructure.Customers;
using ParallelQueries.CoreApp.Infrastructure.Setup;

namespace ParallelQueries.CoreApp.Api.Setup;

public static class CoreAppApiServices
{
    public static async Task AddCoreAppServices(this IServiceCollection services)
    {
        await services.AddCoreAppInfrastructureServices();
        services.AddScoped<IParallelCustomerQuery, ParallelCustomerQuery>();
        services.AddScoped<ISeriesCustomerQuery, SeriesCustomerQuery>();
        services.AddScoped<ICustomerService, CustomerService>();
    }
}