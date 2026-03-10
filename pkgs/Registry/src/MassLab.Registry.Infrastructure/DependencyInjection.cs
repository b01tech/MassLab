using MassLab.Registry.Domain.Interfaces;
using MassLab.Registry.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MassLab.Registry.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddRegistryInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IOwnerRepository, OwnerRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();

        return services;
    }
}
