using MassLib.Shared.Results;

namespace MassLib.Identity.Application.Commands.ActivateUser;

public record ActivateUserCommand(Guid UserId);
