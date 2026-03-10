using MassLab.Registry.Application.DTOs;
using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.Results;
using MassLab.Shared.ValueObject;

namespace MassLab.Registry.Application.Commands.Client.CreateClient;

public class CreateClientHandler(IClientRepository repository)
{
    public async Task<Result<ClientResponse>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        // 1. Create Value Objects
        var legalNameResult = Name.Create(request.LegalName);
        if (legalNameResult.IsFailure) return Result<ClientResponse>.Failure(legalNameResult.Errors);

        Name? businessName = null;
        if (!string.IsNullOrEmpty(request.BusinessName))
        {
            var businessNameResult = Name.Create(request.BusinessName);
            if (businessNameResult.IsFailure) return Result<ClientResponse>.Failure(businessNameResult.Errors);
            businessName = businessNameResult.Data;
        }

        // DocumentNumber logic: Try create Cpf, if fails, try Cnpj
        Result<DocumentNumber> documentResult;
        if (request.DocumentNumber.Length == 11)
        {
            var cpfResult = Cpf.Create(request.DocumentNumber);
            if (cpfResult.IsFailure) return Result<ClientResponse>.Failure(cpfResult.Errors);
            documentResult = Result<DocumentNumber>.Success(cpfResult.Data);
        }
        else
        {
            var cnpjResult = Cnpj.Create(request.DocumentNumber);
            if (cnpjResult.IsFailure) return Result<ClientResponse>.Failure(cnpjResult.Errors);
            documentResult = Result<DocumentNumber>.Success(cnpjResult.Data);
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

        // 2. Create Entity
        var clientResult = Domain.Entities.Client.Create(
            legalNameResult.Data,
            businessName,
            documentResult.Data,
            address,
            email,
            phone
        );

        if (clientResult.IsFailure) return Result<ClientResponse>.Failure(clientResult.Errors);

        // 3. Persist
        await repository.AddAsync(clientResult.Data, cancellationToken);

        return MapToResponse(clientResult.Data);
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
