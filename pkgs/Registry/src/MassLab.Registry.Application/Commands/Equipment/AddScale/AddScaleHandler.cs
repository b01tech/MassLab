using MassLab.Registry.Application.DTOs;
using MassLab.Registry.Domain.Entities;
using MassLab.Registry.Domain.Enums;
using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.Errors;
using MassLab.Shared.Persistence;
using MassLab.Shared.Results;

namespace MassLab.Registry.Application.Commands.Equipment.AddScale;

public record AddScaleCommand(
    Guid ClientId,
    string Tag,
    string Manufacturer,
    string Model,
    string? Identifier,
    float CapMax,
    float Resolution,
    string SerialNumber,
    ScaleClass Class
);

public class AddScaleHandler(IClientRepository repository, IUnitOfWork unitOfWork)
{
    public async Task<Result<ScaleResponse>> Handle(AddScaleCommand request, CancellationToken cancellationToken)
    {
        var client = await repository.GetByIdWithEquipmentsAsync(request.ClientId, cancellationToken);
        if (client == null)
            return Result<ScaleResponse>.Failure(ErrorMessages.CLIENT_NOT_FOUND);

        var scaleResult = Scale.Create(
            request.Tag,
            request.Manufacturer,
            request.Model,
            request.Identifier,
            request.CapMax,
            request.Resolution,
            request.SerialNumber,
            request.Class
        );

        if (scaleResult.IsFailure)
            return Result<ScaleResponse>.Failure(scaleResult.Errors);

        client.AddEquipment(scaleResult.Data);
        
        await repository.UpdateAsync(client, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new ScaleResponse(
            scaleResult.Data.Id,
            scaleResult.Data.Tag,
            scaleResult.Data.Manufacturer,
            scaleResult.Data.Model,
            scaleResult.Data.Identifier,
            scaleResult.Data.History,
            scaleResult.Data.CapMax,
            scaleResult.Data.Resolution,
            scaleResult.Data.SerialNumber,
            scaleResult.Data.Class
        );
    }
}
