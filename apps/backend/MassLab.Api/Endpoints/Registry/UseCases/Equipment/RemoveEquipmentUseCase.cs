using MassLab.Registry.Application.Commands.Equipment.RemoveEquipment;
using Microsoft.AspNetCore.Mvc;

namespace MassLab.Api.Endpoints.Registry.UseCases.Equipment;

public static class RemoveEquipmentUseCase
{
    public static async Task<IResult> ExecuteAsync(
        [FromRoute] Guid id,
        [FromRoute] Guid equipmentId,
        [FromServices] RemoveEquipmentHandler handler,
        CancellationToken ct)
    {
        var command = new RemoveEquipmentCommand(id, equipmentId);
        var result = await handler.Handle(command, ct);
        
        return result.IsSuccess
            ? Results.NoContent()
            : Results.BadRequest(result.Errors);
    }
}
