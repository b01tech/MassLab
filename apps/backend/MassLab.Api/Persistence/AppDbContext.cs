using MassLab.Identity.Domain.Entities;
using MassLab.Identity.Infrastructure.Persistence;
using MassLab.Registry.Domain.Entities;
using MassLab.Registry.Infrastructure.Persistence;
using MassLab.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MassLab.Api.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUnitOfWork, IIdentityDbContext, IRegistryDbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Owner> Owners => Set<Owner>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Equipment> Equipments => Set<Equipment>();

    public Task CommitAsync(CancellationToken cancellationToken = default) => SaveChangesAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Identity.Infrastructure.Persistence.Configurations.UserConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Registry.Infrastructure.Persistence.Configurations.OwnerConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Registry.Infrastructure.Persistence.Configurations.ClientConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
