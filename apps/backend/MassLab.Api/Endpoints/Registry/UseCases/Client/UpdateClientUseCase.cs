using MassLab.Registry.Application.Commands.Client.UpdateClient;
using MassLab.Registry.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MassLab.Api.Endpoints.Registry.UseCases.Client;

public static class UpdateClientUseCase
{
    public static async Task<IResult> ExecuteAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateClientCommand command,
        [FromServices] UpdateClientHandler handler,
        CancellationToken ct)
    {
        if (command.Id != Guid.Empty && command.Id != id)
        {
            return Results.BadRequest("Id in route and body must match");
        }
        
        var commandWithId = command with { Id = id };
        var result = await handler.Handle(commandWithId, ct);
        
        return result.IsSuccess
            ? Results.Ok(result.Data)
            : Results.BadRequest(result.Errors);
    }
}
