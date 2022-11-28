using BeanLog.Shared.Infrastructure.Persistence;
using BeanLog.Shared.Web.Persistence;
using BeanLog.Shared.Web.Persistence.Identity.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace BeanLog.Shared.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedResources(this IServiceCollection services,
        IWebHostEnvironment environment, IConfigurationSection configuration)
    {
        services.AddDbContext<BeanLogDbContext>(options =>
        {
            options
                .UseNpgsql(configuration.GetConnectionString("Database"),
                    o => { o.MigrationsAssembly(typeof(ServiceCollectionExtensions).Assembly.FullName); })
                .UseSnakeCaseNamingConvention();
        });

        services.AddIdentity<User, Role>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Password.RequireDigit = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 4;
        }).AddEntityFrameworkStores<BeanLogDbContext>();

        services.PostConfigure<StaticFileOptions>(options =>
        {
            options.FileProvider ??= environment.WebRootFileProvider;

            var filesProvider = new ManifestEmbeddedFileProvider(typeof(IDbContext).Assembly, "resources");
            options.FileProvider = new CompositeFileProvider(options.FileProvider, filesProvider);
        });

        return services;
    }
}