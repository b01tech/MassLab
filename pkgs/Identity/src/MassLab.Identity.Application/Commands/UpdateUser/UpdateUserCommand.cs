using MassLab.Identity.Application.DTOs;
using MassLab.Identity.Domain.Enums;
using MassLab.Shared.Results;

namespace MassLab.Identity.Application.Commands.UpdateUser;

public record UpdateUserCommand(Guid UserId, string UserName, string Role);
