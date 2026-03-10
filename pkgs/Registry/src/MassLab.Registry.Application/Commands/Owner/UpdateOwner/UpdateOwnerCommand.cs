namespace MassLab.Registry.Application.Commands.Owner.UpdateOwner;

public record UpdateOwnerCommand(
    string LegalName,
    string BusinessName,
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
