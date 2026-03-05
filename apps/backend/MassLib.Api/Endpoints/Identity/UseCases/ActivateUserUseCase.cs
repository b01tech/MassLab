using MassLib.Identity.Application.Commands.ActivateUser;
using Microsoft.AspNetCore.Mvc;

namespace MassLib.Api.Endpoints.Identity.UseCases;

public static class ActivateUserUseCase
{
    public static async Task<IResult> ExecuteAsync([FromRoute] Guid id, [FromServices] ActivateUserHandler handler,
        CancellationToken ct)
    {
        var command = new ActivateUserCommand(id);
        var result = await handler.Handle(command, ct);
        return result.IsSuccess
            ? Results.NoContent()
            : Results.BadRequest(result.Errors);
    }
}
