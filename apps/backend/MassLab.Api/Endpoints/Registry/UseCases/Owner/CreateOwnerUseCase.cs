using MassLab.Registry.Application.Commands.Owner.CreateOwner;
using MassLab.Registry.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MassLab.Api.Endpoints.Registry.UseCases.Owner;

public static class CreateOwnerUseCase
{
    public static async Task<IResult> ExecuteAsync(
        [FromBody] CreateOwnerCommand command,
        [FromServices] CreateOwnerHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(command, ct);
        return result.IsSuccess
            ? Results.Created($"/api/v1/owner", result.Data)
            : Results.BadRequest(result.Errors);
    }
}
