using MassLab.Registry.Application.Commands.Owner.UpdateOwner;
using MassLab.Registry.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MassLab.Api.Endpoints.Registry.UseCases.Owner;

public static class UpdateOwnerUseCase
{
    public static async Task<IResult> ExecuteAsync(
        [FromBody] UpdateOwnerCommand command,
        [FromServices] UpdateOwnerHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(command, ct);
        return result.IsSuccess
            ? Results.Ok(result.Data)
            : Results.BadRequest(result.Errors);
    }
}
