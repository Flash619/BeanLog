using BeanLog.Shared.Web.Persistence;
using BeanLog.Shared.Web.Persistence.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeanLog.Shared.Infrastructure.Persistence;

public class BeanLogDbContext : IdentityDbContext<User, Role, Guid>, IDbContext
{
    public BeanLogDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}