using MassLib.Identity.Application.Commands.DeactivateUser;
using Microsoft.AspNetCore.Mvc;

namespace MassLib.Api.Endpoints.Identity.UseCases;

public static class DeactivateUserUseCase
{
    public static async Task<IResult> ExecuteAsync([FromRoute] Guid id, [FromServices] DeactivateUserHandler handler,
        CancellationToken ct)
    {
        var command = new DeactivateUserCommand(id);
        var result = await handler.Handle(command, ct);
        return result.IsSuccess
            ? Results.NoContent()
            : Results.BadRequest(result.Errors);
    }
}
