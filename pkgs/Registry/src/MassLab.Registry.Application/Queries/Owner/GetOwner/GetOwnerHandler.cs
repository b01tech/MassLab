using MassLab.Registry.Application.DTOs;
using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.Results;

namespace MassLab.Registry.Application.Queries.Owner.GetOwner;

public class GetOwnerHandler(IOwnerRepository repository)
{
    public async Task<Result<OwnerResponse>> Handle(GetOwnerQuery request, CancellationToken cancellationToken)
    {
        var owner = await repository.GetAsync(cancellationToken);
        if (owner == null)
            return Result<OwnerResponse>.Failure("Owner not found");

        return MapToResponse(owner);
    }

    private static OwnerResponse MapToResponse(Domain.Entities.Owner owner)
    {
        return new OwnerResponse(
            owner.Id,
            owner.LegalName.Value,
            owner.BusinessName.Value,
            owner.Cnpj.Formatted,
            owner.Address != null ? new AddressResponse(
                owner.Address.Street,
                owner.Address.Number,
                owner.Address.Complement,
                owner.Address.Neighborhood,
                owner.Address.ZipCode,
                owner.Address.City.Name,
                owner.Address.State.Value
            ) : null,
            owner.Email?.Value,
            owner.Phone?.ToString(),
            owner.LogoUrl
        );
    }
}
