using MassLab.Registry.Application.Commands.Client.AddClientContact;
using MassLab.Registry.Application.Commands.Client.CreateClient;
using MassLab.Registry.Application.Commands.Client.RemoveClientContact;
using MassLab.Registry.Application.Commands.Client.UpdateClient;
using MassLab.Registry.Application.Commands.Owner.CreateOwner;
using MassLab.Registry.Application.Commands.Owner.UpdateOwner;
using MassLab.Registry.Application.Queries.Client.GetClientById;
using MassLab.Registry.Application.Queries.Client.GetClients;
using MassLab.Registry.Application.Queries.Owner.GetOwner;
using Microsoft.Extensions.DependencyInjection;

namespace MassLab.Registry.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddRegistryApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateOwnerHandler>();
        services.AddScoped<UpdateOwnerHandler>();
        services.AddScoped<GetOwnerHandler>();

        services.AddScoped<CreateClientHandler>();
        services.AddScoped<UpdateClientHandler>();
        services.AddScoped<AddClientContactHandler>();
        services.AddScoped<RemoveClientContactHandler>();
        services.AddScoped<GetClientByIdHandler>();
        services.AddScoped<GetClientsHandler>();

        return services;
    }
}
