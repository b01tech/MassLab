using MassLab.Registry.Domain.Entities;
using MassLab.Shared.ValueObject;

namespace MassLab.Registry.Domain.Interfaces;

public interface IClientRepository
{
    Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Client?> GetByIdWithEquipmentsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Client>> GetAllAsync(int page, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default);
    Task<int> GetTotalCountAsync(string? searchTerm = null, CancellationToken cancellationToken = default);
    Task AddAsync(Client client, CancellationToken cancellationToken = default);
    Task UpdateAsync(Client client, CancellationToken cancellationToken = default);
}
