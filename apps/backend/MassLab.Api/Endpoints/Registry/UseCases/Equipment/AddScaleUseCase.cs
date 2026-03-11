using MassLab.Registry.Application.Commands.Equipment.AddScale;
using MassLab.Registry.Application.DTOs;
using MassLab.Registry.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MassLab.Api.Endpoints.Registry.UseCases.Equipment;

public static class AddScaleUseCase
{
    public record AddScaleRequest(
        string Tag,
        string Manufacturer,
        string Model,
        string? Identifier,
        float CapMax,
        float Resolution,
        string SerialNumber,
        ScaleClass Class
    );

    public static async Task<IResult> ExecuteAsync(
        [FromRoute] Guid id,
        [FromBody] AddScaleRequest request,
        [FromServices] AddScaleHandler handler,
        CancellationToken ct)
    {
        var command = new AddScaleCommand(
            id,
            request.Tag,
            request.Manufacturer,
            request.Model,
            request.Identifier,
            request.CapMax,
            request.Resolution,
            request.SerialNumber,
            request.Class
        );

        var result = await handler.Handle(command, ct);
        
        return result.IsSuccess
            ? Results.Created($"/api/v1/clients/{id}/equipments/{result.Data.Id}", result.Data)
            : Results.BadRequest(result.Errors);
    }
}
