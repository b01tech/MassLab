using MassLab.Registry.Domain.Interfaces;
using MassLab.Registry.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MassLab.Registry.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddRegistryInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IOwnerRepository, OwnerRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();

        return services;
    }
}
