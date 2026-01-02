using Microsoft.EntityFrameworkCore;
using ParallelQueries.SharedKernel.Configuration;
namespace ParallelQueries.CoreApp.Infrastructure;

public static class CoreDbContextBuilder
{
    public static CoreDbContext Build()
    {
        var options = new DbContextOptionsBuilder<CoreDbContext>();
        options.UseSqlServer(CoreAppInfrastructureConfig.Instance.ConnectionString);
        return new CoreDbContext(options.Options);
    }
}

