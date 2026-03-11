using MassLab.Registry.Application.Commands.Client.CreateClient;
using MassLab.Registry.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MassLab.Api.Endpoints.Registry.UseCases.Client;

public static class CreateClientUseCase
{
    public static async Task<IResult> ExecuteAsync(
        [FromBody] CreateClientCommand command,
        [FromServices] CreateClientHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(command, ct);
        return result.IsSuccess
            ? Results.Created($"/api/v1/clients/{result.Data.Id}", result.Data)
            : Results.BadRequest(result.Errors);
    }
}
