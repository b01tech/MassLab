using MassLab.Registry.Domain.Entities;
using MassLab.Registry.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MassLab.Registry.Infrastructure.Persistence.Repositories;

public class ClientRepository(IRegistryDbContext context) : IClientRepository
{
    public async Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Clients
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Client>> GetAllAsync(int page, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        var query = context.Clients.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c => c.LegalName.Value.Contains(searchTerm) || (c.BusinessName != null && c.BusinessName.Value.Contains(searchTerm)));
        }

        return await query
            .OrderBy(c => c.LegalName.Value)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        var query = context.Clients.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c => c.LegalName.Value.Contains(searchTerm) || (c.BusinessName != null && c.BusinessName.Value.Contains(searchTerm)));
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task AddAsync(Client client, CancellationToken cancellationToken = default)
    {
        await context.Clients.AddAsync(client, cancellationToken);
    }

    public Task UpdateAsync(Client client, CancellationToken cancellationToken = default)
    {
        context.Clients.Update(client);
        return Task.CompletedTask;
    }
}
