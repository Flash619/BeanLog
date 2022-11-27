using BeanLog.Contexts.Identity.Domain.Entities;
using BeanLog.Contexts.Identity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BeanLog.Contexts.Identity.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddIdentityContext(this IServiceCollection services, IConfigurationSection configuration)
    {
        services.AddDbContext<BeanLogIdentityDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Database"), o =>
            {
                o.MigrationsAssembly(typeof(ServiceCollectionExtensions).Assembly.FullName);
            });
        });
        
        services.AddIdentity<User, Role>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 4;
            options.Stores.ProtectPersonalData = false;
        }).AddEntityFrameworkStores<BeanLogIdentityDbContext>();
    }
}