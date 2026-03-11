using MassLab.Registry.Application.DTOs;
using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.Errors;
using MassLab.Shared.Results;

namespace MassLab.Registry.Application.Queries.Client.GetClientById;

public class GetClientByIdHandler(IClientRepository repository)
{
    public async Task<Result<ClientResponse>> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        var client = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (client == null)
            return Result<ClientResponse>.Failure(ErrorMessages.CLIENT_NOT_FOUND);

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
