using BeanLog.Modules.Identity.Domain.Roles;
using BeanLog.Modules.Identity.Domain.Users;
using BeanLog.Modules.Identity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BeanLog.Modules.Identity.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection serviceCollection, IConfigurationSection configuration)
    {
        serviceCollection.AddIdentity<User, Role>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Password.RequireDigit = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 4;
        }).AddEntityFrameworkStores<BeanLogIdentityDbContext>();
        
        serviceCollection.AddDbContext<BeanLogIdentityDbContext>(options =>
        {
            options
                .UseNpgsql(configuration.GetConnectionString("Database"),
                    o => { o.MigrationsAssembly(typeof(ServiceCollectionExtensions).Assembly.FullName); })
                .UseSnakeCaseNamingConvention();
        });
        
        return serviceCollection;
    }
}