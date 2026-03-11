using MassLab.Registry.Application.Commands.Client.RemoveClientContact;
using MassLab.Registry.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MassLab.Api.Endpoints.Registry.UseCases.Client;

public static class RemoveClientContactUseCase
{
    public static async Task<IResult> ExecuteAsync(
        [FromRoute] Guid id,
        [FromQuery] string email,
        [FromQuery] string phone,
        [FromServices] RemoveClientContactHandler handler,
        CancellationToken ct)
    {
        var command = new RemoveClientContactCommand(id, email, phone);
        var result = await handler.Handle(command, ct);
        
        return result.IsSuccess
            ? Results.NoContent()
            : Results.BadRequest(result.Errors);
    }
}
