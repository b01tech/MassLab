using MassLab.Registry.Application.DTOs;
using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.Errors;
using MassLab.Shared.Results;
using MassLab.Shared.ValueObject;

namespace MassLab.Registry.Application.Commands.Client.UpdateClient;

public class UpdateClientHandler(IClientRepository repository)
{
    public async Task<Result<ClientResponse>> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var client = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (client == null)
            return Result<ClientResponse>.Failure(ErrorMessages.CLIENT_NOT_FOUND);

        var legalNameResult = Name.Create(request.LegalName);
        if (legalNameResult.IsFailure) return Result<ClientResponse>.Failure(legalNameResult.Errors);

        Name? businessName = null;
        if (!string.IsNullOrEmpty(request.BusinessName))
        {
            var businessNameResult = Name.Create(request.BusinessName);
            if (businessNameResult.IsFailure) return Result<ClientResponse>.Failure(businessNameResult.Errors);
            businessName = businessNameResult.Data;
        }

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
            if (addressResult.IsFailure) return Result<ClientResponse>.Failure(addressResult.Errors);
            address = addressResult.Data;
        }

        Email? email = null;
        if (!string.IsNullOrEmpty(request.Email))
        {
            var emailResult = Email.Create(request.Email);
            if (emailResult.IsFailure) return Result<ClientResponse>.Failure(emailResult.Errors);
            email = emailResult.Data;
        }

        Phone? phone = null;
        if (!string.IsNullOrEmpty(request.Phone))
        {
            var phoneResult = Phone.Create(request.Phone);
            if (phoneResult.IsFailure) return Result<ClientResponse>.Failure(phoneResult.Errors);
            phone = phoneResult.Data;
        }

        client.Update(
            legalNameResult.Data,
            businessName,
            address,
            email,
            phone
        );

        await repository.UpdateAsync(client, cancellationToken);

        return MapToResponse(client);
    }

    private static ClientResponse MapToResponse(Domain.Entities.Client client)
    {
        return new ClientResponse(
            client.Id,
            client.LegalName.Value,
            client.BusinessName?.Value ?? "",
            client.DocumentNumber?.Value,
            client.Address != null ? new AddressResponse(
                client.Address.Street,
                client.Address.Number,
                client.Address.Complement,
                client.Address.Neighborhood,
                client.Address.ZipCode,
                client.Address.City.Name,
                client.Address.State.Value
            ) : null,
            client.Email?.Value,
            client.Phone?.ToString(),
            client.Contacts.Select(c => new ContactResponse(c.Name.Value, c.Email.Value, c.Phone.ToString())).ToList()
        );
    }
}
