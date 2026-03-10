using MassLab.Registry.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MassLab.Registry.Infrastructure.Persistence;

public interface IRegistryDbContext
{
    DbSet<Owner> Owners { get; }
    DbSet<Client> Clients { get; }
    
}
