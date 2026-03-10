using MassLab.Registry.Application.DTOs;
using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.Results;

namespace MassLab.Registry.Application.Queries.Client.GetClients;

public class GetClientsHandler(IClientRepository repository)
{
    public async Task<Result<PaginatedResponse<ClientResponse>>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
    {
        var clients = await repository.GetAllAsync(request.Page, request.PageSize, request.SearchTerm, cancellationToken);
        var totalCount = await repository.GetTotalCountAsync(request.SearchTerm, cancellationToken);

        var responseItems = clients.Select(MapToResponse).ToList();

        return new PaginatedResponse<ClientResponse>(responseItems, request.Page, request.PageSize, totalCount);
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
