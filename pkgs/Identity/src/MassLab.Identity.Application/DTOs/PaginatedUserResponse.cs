using MassLab.Shared.ValueObject;

namespace MassLab.Identity.Application.DTOs;

public record PaginatedUserResponse(Pagination Pagination, IEnumerable<UserResponse> Data);
