using MassLab.Registry.Application.DTOs;
using MassLab.Registry.Application.Queries.Client.GetClients;
using Microsoft.AspNetCore.Mvc;

namespace MassLab.Api.Endpoints.Registry.UseCases.Client;

public static class GetClientsUseCase
{
    public static async Task<IResult> ExecuteAsync(
        [AsParameters] GetClientsQuery query,
        [FromServices] GetClientsHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(query, ct);
        return result.IsSuccess
            ? Results.Ok(result.Data)
            : Results.BadRequest(result.Errors);
    }
}
