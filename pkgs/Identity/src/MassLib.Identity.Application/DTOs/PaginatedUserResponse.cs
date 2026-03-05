using MassLib.Shared.ValueObject;

namespace MassLib.Identity.Application.DTOs;

public record PaginatedUserResponse(IEnumerable<UserResponse> Data, Pagination Pagination);
