namespace MassLab.Registry.Application.Commands.Client.RemoveClientContact;

public record RemoveClientContactCommand(
    Guid ClientId,
    string Email,
    string Phone
);
