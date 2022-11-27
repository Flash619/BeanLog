using Microsoft.EntityFrameworkCore;

namespace BeanLog.Shared.Web.Persistence;

public interface IDbContext
{
    DbSet<T> Set<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}