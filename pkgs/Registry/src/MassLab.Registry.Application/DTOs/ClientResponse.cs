namespace MassLab.Registry.Application.DTOs;

public record ClientResponse(
    Guid Id,
    string LegalName,
    string BusinessName,
    string? DocumentNumber,
    AddressResponse? Address,
    string? Email,
    string? Phone,
    List<ContactResponse> Contacts
);
