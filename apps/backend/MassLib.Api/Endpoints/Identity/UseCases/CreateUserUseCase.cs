using MassLib.Identity.Application.Commands.CreateUser;
using Microsoft.AspNetCore.Mvc;

namespace MassLib.Api.Endpoints.Identity.UseCases;

public static class CreateUserUseCase
{
    public static async Task<IResult> ExecuteAsync([FromBody] CreateUserCommand command, [FromServices] CreateUserHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(command, ct);
        return result.IsSuccess
            ? Results.Created($"/users/{result.Data.Id}", result.Data)
            : Results.BadRequest(result.Errors);
    }
}
