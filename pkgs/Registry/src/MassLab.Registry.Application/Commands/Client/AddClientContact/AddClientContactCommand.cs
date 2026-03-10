namespace MassLab.Registry.Application.Commands.Client.AddClientContact;

public record AddClientContactCommand(
    Guid ClientId,
    string Name,
    string Email,
    string Phone
);
