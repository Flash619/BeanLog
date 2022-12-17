using Microsoft.EntityFrameworkCore;

namespace BeanLog.Modules.Core.Application.Persistence;

public interface IDbContext
{
    DbSet<T> Set<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}