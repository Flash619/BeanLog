using BeanLog.Modules.Core.Application.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BeanLog.Modules.Core.Infrastructure.Persistence;

public class BeanLogDbContext : DbContext, IDbContext
{
    public BeanLogDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}