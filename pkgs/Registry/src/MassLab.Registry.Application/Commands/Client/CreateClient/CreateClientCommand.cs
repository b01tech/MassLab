namespace MassLab.Registry.Application.Commands.Client.CreateClient;

public record CreateClientCommand(
    string LegalName,
    string? BusinessName,
    string DocumentNumber,
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
