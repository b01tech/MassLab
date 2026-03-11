using MassLab.Registry.Application.DTOs;
using MassLab.Registry.Domain.Entities;
using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.Results;

namespace MassLab.Registry.Application.Queries.Equipment.GetClientEquipments;

public record GetClientEquipmentsQuery(Guid ClientId);

public class GetClientEquipmentsHandler(IClientRepository repository)
{
    public async Task<Result<IEnumerable<EquipmentResponse>>> Handle(GetClientEquipmentsQuery request, CancellationToken cancellationToken)
    {
        var client = await repository.GetByIdWithEquipmentsAsync(request.ClientId, cancellationToken);
        if (client == null)
            return Result<IEnumerable<EquipmentResponse>>.Failure("Client not found");

        var responses = client.Equipments.Select(MapToResponse).ToList();
        return Result<IEnumerable<EquipmentResponse>>.Success(responses);
    }

    private static EquipmentResponse MapToResponse(Domain.Entities.Equipment equipment)
    {
        if (equipment is Scale scale)
        {
            return new ScaleResponse(
                scale.Id,
                scale.Tag,
                scale.Manufacturer,
                scale.Model,
                scale.Identifier,
                scale.History,
                scale.CapMax,
                scale.Resolution,
                scale.SerialNumber,
                scale.Class
            );
        }

        return new EquipmentResponse(
            equipment.Id,
            equipment.Tag,
            equipment.Manufacturer,
            equipment.Model,
            equipment.Type,
            equipment.Identifier,
            equipment.History
        );
    }
}
