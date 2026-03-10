namespace MassLab.Registry.Application.DTOs;

public record OwnerResponse(
    Guid Id,
    string LegalName,
    string BusinessName,
    string Cnpj,
    AddressResponse? Address,
    string? Email,
    string? Phone,
    string? LogoUrl
);
