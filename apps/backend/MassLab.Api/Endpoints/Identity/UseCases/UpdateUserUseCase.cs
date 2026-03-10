using MassLab.Identity.Application.Commands.UpdateUser;
using MassLab.Shared.Errors;
using Microsoft.AspNetCore.Mvc;

namespace MassLab.Api.Endpoints.Identity.UseCases;

public static class UpdateUserUseCase
{
    public static async Task<IResult> ExecuteAsync([FromRoute] Guid id, [FromBody] UpdateUserCommand command, [FromServices] UpdateUserHandler handler,
        CancellationToken ct)
    {
        if (id != command.UserId) return Results.BadRequest(ErrorMessages.ID_MISMATCH);

        var result = await handler.Handle(command, ct);
        return result.IsSuccess
            ? Results.Ok(result.Data)
            : Results.BadRequest(result.Errors);
    }
}
