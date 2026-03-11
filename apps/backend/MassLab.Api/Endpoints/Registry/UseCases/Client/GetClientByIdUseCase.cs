using MassLab.Registry.Application.DTOs;
using MassLab.Registry.Application.Queries.Client.GetClientById;
using Microsoft.AspNetCore.Mvc;

namespace MassLab.Api.Endpoints.Registry.UseCases.Client;

public static class GetClientByIdUseCase
{
    public static async Task<IResult> ExecuteAsync(
        [FromRoute] Guid id,
        [FromServices] GetClientByIdHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(new GetClientByIdQuery(id), ct);
        return result.IsSuccess
            ? Results.Ok(result.Data)
            : Results.NotFound(result.Errors);
    }
}
