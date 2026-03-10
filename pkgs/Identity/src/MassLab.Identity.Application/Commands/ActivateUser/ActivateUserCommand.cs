using MassLab.Shared.Results;

namespace MassLab.Identity.Application.Commands.ActivateUser;

public record ActivateUserCommand(Guid UserId);
