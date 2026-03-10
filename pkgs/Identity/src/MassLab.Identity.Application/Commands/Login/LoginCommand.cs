using MassLab.Identity.Application.DTOs;
using MassLab.Shared.Results;

namespace MassLab.Identity.Application.Commands.Login;

public record LoginCommand(string UserName, string Password);
