using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.Errors;
using MassLab.Shared.Persistence;
using MassLab.Shared.Results;

namespace MassLab.Registry.Application.Commands.Equipment.RemoveEquipment;

public record RemoveEquipmentCommand(Guid ClientId, Guid EquipmentId);

public class RemoveEquipmentHandler(IClientRepository repository, IUnitOfWork unitOfWork)
{
    public async Task<Result> Handle(RemoveEquipmentCommand request, CancellationToken cancellationToken)
    {
        var client = await repository.GetByIdWithEquipmentsAsync(request.ClientId, cancellationToken);
        if (client == null)
            return Result.Failure(ErrorMessages.CLIENT_NOT_FOUND);

        var equipment = client.Equipments.FirstOrDefault(e => e.Id == request.EquipmentId);
        if (equipment == null)
            return Result.Failure(ErrorMessages.EQUIPMENT_NOT_FOUND);

        client.RemoveEquipment(equipment);
        
        await repository.UpdateAsync(client, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
