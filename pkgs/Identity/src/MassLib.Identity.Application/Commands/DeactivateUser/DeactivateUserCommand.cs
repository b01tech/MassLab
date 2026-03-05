using MassLib.Shared.Results;

namespace MassLib.Identity.Application.Commands.DeactivateUser;

public record DeactivateUserCommand(Guid UserId);
