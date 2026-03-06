namespace MassLib.Identity.Application.Queries.GetUsers;

public record GetUsersQuery(int Page, int PageSize, string? SearchTerm = null);
