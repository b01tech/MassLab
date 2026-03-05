using MassLib.Shared.Results;

namespace MassLib.Identity.Application.Commands.ChangePassword;

public record ChangePasswordCommand(Guid UserId, string NewPassword);
