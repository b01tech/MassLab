using MassLib.Shared.ValueObject;

namespace MassLib.Identity.Application.DTOs;

public record PaginatedUserResponse(Pagination Pagination, IEnumerable<UserResponse> Data);
