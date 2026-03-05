using MassLib.Identity.Application.DTOs;
using MassLib.Identity.Domain.Enums;
using MassLib.Identity.Domain.Interfaces;
using MassLib.Shared.Errors;
using MassLib.Shared.Persistence;
using MassLib.Shared.Results;

namespace MassLib.Identity.Application.Commands.UpdateUser;

public class UpdateUserHandler(IUserRepository repository, IUnitOfWork uow)
{
    public async Task<Result<UserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<UserRole>(request.Role, true, out var role) || !Enum.IsDefined(typeof(UserRole), role))
            return Result<UserResponse>.Failure(ErrorMessages.ROLE_INVALID);

        var user = await repository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result<UserResponse>.Failure(ErrorMessages.USER_NOT_FOUND);

        var updateResult = user.Update(request.UserName, role);
        if (updateResult.IsFailure)
            return Result<UserResponse>.Failure(updateResult.Errors);

        await repository.UpdateAsync(user, cancellationToken);
        await uow.CommitAsync();

        return new UserResponse(user.Id, user.UserName.Value, user.Role.ToString(), user.Active);
    }
}
