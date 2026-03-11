using MassLab.Registry.Domain.Interfaces;
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
            return Result.Failure("Client not found");

        var equipment = client.Equipments.FirstOrDefault(e => e.Id == request.EquipmentId);
        if (equipment == null)
            return Result.Failure("Equipment not found");

        client.RemoveEquipment(equipment);
        
        await repository.UpdateAsync(client, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
