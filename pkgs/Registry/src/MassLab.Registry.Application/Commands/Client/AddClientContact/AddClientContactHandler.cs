using MassLab.Registry.Application.DTOs;
using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.Errors;
using MassLab.Shared.Results;
using MassLab.Shared.ValueObject;

namespace MassLab.Registry.Application.Commands.Client.AddClientContact;

public class AddClientContactHandler(IClientRepository repository)
{
    public async Task<Result<ClientResponse>> Handle(AddClientContactCommand request, CancellationToken cancellationToken)
    {
        var client = await repository.GetByIdAsync(request.ClientId, cancellationToken);
        if (client == null)
            return Result<ClientResponse>.Failure(ErrorMessages.CLIENT_NOT_FOUND);

        var contactResult = Contact.Create(request.Name, request.Email, request.Phone);
        if (contactResult.IsFailure) return Result<ClientResponse>.Failure(contactResult.Errors);

        client.AddContact(contactResult.Data);

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
