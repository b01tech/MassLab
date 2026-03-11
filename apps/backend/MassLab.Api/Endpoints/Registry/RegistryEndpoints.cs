using MassLab.Api.Endpoints.Registry.UseCases.Client;
using MassLab.Api.Endpoints.Registry.UseCases.Owner;
using MassLab.Registry.Application.DTOs;

namespace MassLab.Api.Endpoints.Registry;

public static class RegistryEndpoints
{
    public static void MapRegistryEndpoints(this IEndpointRouteBuilder app)
    {
        var ownerGroup = app.MapGroup("api/v1/owner")
            .WithTags("Owner");

        ownerGroup.MapPost("", CreateOwnerUseCase.ExecuteAsync)
            .Produces<OwnerResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Create Owner");

        ownerGroup.MapPut("", UpdateOwnerUseCase.ExecuteAsync)
            .Produces<OwnerResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Update Owner");

        ownerGroup.MapGet("", GetOwnerUseCase.ExecuteAsync)
            .Produces<OwnerResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get Owner");

        var clientGroup = app.MapGroup("api/v1/clients")
            .WithTags("Client");

        clientGroup.MapPost("", CreateClientUseCase.ExecuteAsync)
            .Produces<ClientResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Create Client");

        clientGroup.MapPut("/{id:guid}", UpdateClientUseCase.ExecuteAsync)
            .Produces<ClientResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Update Client");

        clientGroup.MapGet("", GetClientsUseCase.ExecuteAsync)
            .Produces<PaginatedResponse<ClientResponse>>(StatusCodes.Status200OK)
            .WithSummary("Get Clients");

        clientGroup.MapGet("/{id:guid}", GetClientByIdUseCase.ExecuteAsync)
            .Produces<ClientResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get Client by Id");

        clientGroup.MapPost("/{id:guid}/contacts", AddClientContactUseCase.ExecuteAsync)
            .Produces<ClientResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Add Contact to Client");

        clientGroup.MapDelete("/{id:guid}/contacts", RemoveClientContactUseCase.ExecuteAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Remove Contact from Client");
    }
}
