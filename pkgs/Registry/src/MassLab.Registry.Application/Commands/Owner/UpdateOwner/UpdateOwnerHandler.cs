using MassLab.Registry.Application.DTOs;
using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.Errors;
using MassLab.Shared.Results;
using MassLab.Shared.ValueObject;

namespace MassLab.Registry.Application.Commands.Owner.UpdateOwner;

public class UpdateOwnerHandler(IOwnerRepository repository)
{
    public async Task<Result<OwnerResponse>> Handle(UpdateOwnerCommand request, CancellationToken cancellationToken)
    {
        var owner = await repository.GetAsync(cancellationToken);
        if (owner == null)
            return Result<OwnerResponse>.Failure(ErrorMessages.OWNER_NOT_FOUND);

        var legalNameResult = Name.Create(request.LegalName);
        var businessNameResult = Name.Create(request.BusinessName);
        
        Address? address = null;
        if (!string.IsNullOrEmpty(request.ZipCode))
        {
            var addressResult = Address.Create(
                request.Street ?? "",
                request.Number ?? "",
                request.Complement ?? "",
                request.Neighborhood ?? "",
                request.ZipCode,
                request.City ?? "",
                request.State ?? ""
            );
            if (addressResult.IsFailure) return Result<OwnerResponse>.Failure(addressResult.Errors);
            address = addressResult.Data;
        }

        Email? email = null;
        if (!string.IsNullOrEmpty(request.Email))
        {
            var emailResult = Email.Create(request.Email);
            if (emailResult.IsFailure) return Result<OwnerResponse>.Failure(emailResult.Errors);
            email = emailResult.Data;
        }

        Phone? phone = null;
        if (!string.IsNullOrEmpty(request.Phone))
        {
            var phoneResult = Phone.Create(request.Phone);
            if (phoneResult.IsFailure) return Result<OwnerResponse>.Failure(phoneResult.Errors);
            phone = phoneResult.Data;
        }

        if (legalNameResult.IsFailure) return Result<OwnerResponse>.Failure(legalNameResult.Errors);
        if (businessNameResult.IsFailure) return Result<OwnerResponse>.Failure(businessNameResult.Errors);

        owner.Update(
            legalNameResult.Data,
            businessNameResult.Data,
            address,
            email,
            phone,
            request.LogoUrl
        );

        await repository.UpdateAsync(owner, cancellationToken);

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
