using MassLab.Identity.Domain.Entities;
using MassLab.Identity.Infrastructure.Persistence;
using MassLab.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MassLab.Api.Persistence;

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
