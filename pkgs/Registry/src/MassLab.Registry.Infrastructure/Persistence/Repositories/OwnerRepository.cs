using MassLab.Registry.Domain.Entities;
using MassLab.Registry.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MassLab.Registry.Infrastructure.Persistence.Repositories;

public class OwnerRepository(IRegistryDbContext context) : IOwnerRepository
{
    public async Task<Owner?> GetAsync(CancellationToken cancellationToken = default)
    {
        return await context.Owners
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(Owner owner, CancellationToken cancellationToken = default)
    {
        await context.Owners.AddAsync(owner, cancellationToken);
    }

    public Task UpdateAsync(Owner owner, CancellationToken cancellationToken = default)
    {
        context.Owners.Update(owner);
        return Task.CompletedTask;
    }
}
