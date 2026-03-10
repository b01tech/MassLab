using MassLab.Identity.Application.DTOs;
using MassLab.Identity.Domain.Interfaces;
using MassLab.Shared.Errors;
using MassLab.Shared.Results;
using MassLab.Shared.ValueObject;

namespace MassLab.Identity.Application.Queries.GetUsers;

public class GetUsersHandler(IUserRepository repository)
{
    public async Task<Result<PaginatedUserResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var total = await repository.GetTotalCountAsync(request.SearchTerm, cancellationToken);
        var pagination = Pagination.Create(request.Page, request.PageSize, total);
        var users = await repository.GetAllAsync(pagination.Page, pagination.PageSize, request.SearchTerm, cancellationToken);
        var response = users.Select(user => new UserResponse(user.Id, user.UserName.Value, user.Role.ToString(), user.Active));

        return new PaginatedUserResponse(Pagination: pagination, Data: response);
    }
}
