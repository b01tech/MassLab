using MassLab.Identity.Domain.Enums;

namespace MassLab.Identity.Application.DTOs;

public record UserResponse(Guid Id, string UserName, string Role, bool Active);
