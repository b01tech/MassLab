using MassLab.Shared.Results;

namespace MassLab.Identity.Application.Commands.DeactivateUser;

public record DeactivateUserCommand(Guid UserId);
