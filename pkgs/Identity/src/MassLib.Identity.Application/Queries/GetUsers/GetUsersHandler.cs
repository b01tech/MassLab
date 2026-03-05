using MassLib.Identity.Application.DTOs;
using MassLib.Identity.Domain.Interfaces;
using MassLib.Shared.Errors;
using MassLib.Shared.Results;
using MassLib.Shared.ValueObject;

namespace MassLib.Identity.Application.Queries.GetUsers;

public class GetUsersHandler(IUserRepository repository)
{
    public async Task<Result<PaginatedUserResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var total = await repository.GetTotalCountAsync(cancellationToken);
        var pagination = Pagination.Create(request.Page, request.PageSize, total);
        var users = await repository.GetAllAsync(pagination.Page, pagination.PageSize, cancellationToken);
        var response = users.Select(user => new UserResponse(user.Id, user.UserName.Value, user.Role, user.Active));

        return new PaginatedUserResponse(Data: response, Pagination: pagination);
    }
}
