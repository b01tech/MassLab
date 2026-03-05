using MassLib.Identity.Domain.Enums;

namespace MassLib.Identity.Application.DTOs;

public record UserResponse(Guid Id, string UserName, string Role, bool Active);
