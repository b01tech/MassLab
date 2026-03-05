using MassLib.Identity.Application.Commands.Login;
using Microsoft.AspNetCore.Mvc;

namespace MassLib.Api.Endpoints.Identity.UseCases;

public static class LoginUseCase
{
    public static async Task<IResult> ExecuteAsync([FromBody] LoginCommand command, [FromServices] LoginHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(command, ct);
        return result.IsSuccess
            ? Results.Ok(result.Data)
            : Results.Unauthorized();
    }
}
