using BeanLog.Modules.Core.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BeanLog.Modules.Core.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreModule(this IServiceCollection services, IConfigurationSection configuration)
    {
        services.AddDbContext<BeanLogDbContext>(options =>
        {
            options
                .UseNpgsql(configuration.GetConnectionString("Database"),
                    o => { o.MigrationsAssembly(typeof(ServiceCollectionExtensions).Assembly.FullName); })
                .UseSnakeCaseNamingConvention();
        });

        return services;
    }
}