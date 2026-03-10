using MassLab.Registry.Domain.Entities;

namespace MassLab.Registry.Domain.Interfaces;

public interface IOwnerRepository
{
    Task<Owner?> GetAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Owner owner, CancellationToken cancellationToken = default);
    Task UpdateAsync(Owner owner, CancellationToken cancellationToken = default);
}
