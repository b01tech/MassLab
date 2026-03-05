using MassLib.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MassLib.Identity.Infrastructure.Persistence;

public interface IIdentityDbContext
{
    DbSet<User> Users { get; }
}
