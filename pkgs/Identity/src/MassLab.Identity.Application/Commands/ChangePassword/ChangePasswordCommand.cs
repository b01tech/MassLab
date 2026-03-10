using MassLab.Shared.Results;

namespace MassLab.Identity.Application.Commands.ChangePassword;

public record ChangePasswordCommand(Guid UserId, string NewPassword);
