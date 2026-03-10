using MassLab.Identity.Application.Commands.ChangePassword;
using MassLab.Shared.Errors;
using Microsoft.AspNetCore.Mvc;

namespace MassLab.Api.Endpoints.Identity.UseCases;

public static class ChangePasswordUseCase
{
    public static async Task<IResult> ExecuteAsync([FromRoute] Guid id, [FromBody] ChangePasswordCommand command, [FromServices] ChangePasswordHandler handler,
        CancellationToken ct)
    {
        if (id != command.UserId) return Results.BadRequest(ErrorMessages.ID_MISMATCH);

        var result = await handler.Handle(command, ct);
        return result.IsSuccess
            ? Results.NoContent()
            : Results.BadRequest(result.Errors);
    }
}
