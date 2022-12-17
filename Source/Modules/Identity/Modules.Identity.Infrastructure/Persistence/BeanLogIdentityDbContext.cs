using BeanLog.Modules.Identity.Domain.Roles;
using BeanLog.Modules.Identity.Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeanLog.Modules.Identity.Infrastructure.Persistence;

public class BeanLogIdentityDbContext : IdentityDbContext<User, Role, Guid>
{
    public BeanLogIdentityDbContext(DbContextOptions options) : base(options)
    {
    }
}