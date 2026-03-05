using MassLib.Identity.Domain.Enums;

namespace MassLib.Identity.Application.DTOs;

public record UserResponse(Guid Id, string UserName, UserRole Role, bool Active);
