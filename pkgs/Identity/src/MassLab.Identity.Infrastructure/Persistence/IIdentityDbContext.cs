using MassLab.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MassLab.Identity.Infrastructure.Persistence;

public interface IIdentityDbContext
{
    DbSet<User> Users { get; }
}
