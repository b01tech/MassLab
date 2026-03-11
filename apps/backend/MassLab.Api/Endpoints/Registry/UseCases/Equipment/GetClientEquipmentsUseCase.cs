using MassLab.Registry.Application.DTOs;
using MassLab.Registry.Application.Queries.Equipment.GetClientEquipments;
using Microsoft.AspNetCore.Mvc;

namespace MassLab.Api.Endpoints.Registry.UseCases.Equipment;

public static class GetClientEquipmentsUseCase
{
    public static async Task<IResult> ExecuteAsync(
        [FromRoute] Guid id,
        [FromServices] GetClientEquipmentsHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(new GetClientEquipmentsQuery(id), ct);
        return result.IsSuccess
            ? Results.Ok(result.Data)
            : Results.BadRequest(result.Errors);
    }
}
