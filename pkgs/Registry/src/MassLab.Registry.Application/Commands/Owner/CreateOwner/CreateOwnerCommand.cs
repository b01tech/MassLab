namespace MassLab.Registry.Application.Commands.Owner.CreateOwner;

public record CreateOwnerCommand(
    string LegalName,
    string BusinessName,
    string Cnpj,
    string? Street,
    string? Number,
    string? Complement,
    string? Neighborhood,
    string? ZipCode,
    string? City,
    string? State,
    string? Email,
    string? Phone,
    string? LogoUrl
);
