using MassLib.Identity.Domain.Entities;
using MassLib.Identity.Infrastructure.Persistence;
using MassLib.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MassLib.Api.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUnitOfWork, IIdentityDbContext
{
    public DbSet<User> Users => Set<User>();

    public Task CommitAsync(CancellationToken cancellationToken = default) => SaveChangesAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Identity.Infrastructure.Persistence.Configurations.UserConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
