using MassLib.Identity.Application.DTOs;
using MassLib.Shared.Results;

namespace MassLib.Identity.Application.Commands.Login;

public record LoginCommand(string UserName, string Password);
