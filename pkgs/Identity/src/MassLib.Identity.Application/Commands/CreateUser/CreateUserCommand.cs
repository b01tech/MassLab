using MassLib.Identity.Application.DTOs;
using MassLib.Identity.Domain.Enums;
using MassLib.Shared.Results;

namespace MassLib.Identity.Application.Commands.CreateUser;

public record CreateUserCommand(string UserName, string Password, string Role);
