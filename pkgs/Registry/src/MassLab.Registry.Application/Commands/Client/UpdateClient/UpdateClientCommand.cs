namespace MassLab.Registry.Application.Commands.Client.UpdateClient;

public record UpdateClientCommand(
    Guid Id,
    string LegalName,
    string? BusinessName,
    string? Street,
    string? Number,
    string? Complement,
    string? Neighborhood,
    string? ZipCode,
    string? City,
    string? State,
    string? Email,
    string? Phone
);
