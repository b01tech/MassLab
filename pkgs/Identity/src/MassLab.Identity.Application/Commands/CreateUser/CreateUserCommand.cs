using MassLab.Identity.Application.DTOs;
using MassLab.Identity.Domain.Enums;
using MassLab.Shared.Results;

namespace MassLab.Identity.Application.Commands.CreateUser;

public record CreateUserCommand(string UserName, string Password, string Role);
