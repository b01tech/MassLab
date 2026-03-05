using MassLib.Identity.Application.DTOs;
using MassLib.Identity.Domain.Enums;
using MassLib.Shared.Results;

namespace MassLib.Identity.Application.Commands.UpdateUser;

public record UpdateUserCommand(Guid UserId, string UserName, string Role);
