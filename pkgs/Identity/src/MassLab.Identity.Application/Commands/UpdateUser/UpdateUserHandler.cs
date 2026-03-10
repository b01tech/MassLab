using MassLab.Identity.Application.DTOs;
using MassLab.Identity.Domain.Interfaces;
using MassLab.Shared.Errors;
using MassLab.Shared.Persistence;
using MassLab.Shared.Results;

namespace MassLab.Identity.Application.Commands.UpdateUser;

public class UpdateUserHandler(IUserRepository repository, IUnitOfWork uow)
{
    public async Task<Result<UserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result<UserResponse>.Failure(ErrorMessages.USER_NOT_FOUND);

        var updateResult = user.Update(request.UserName, request.Role);
        if (updateResult.IsFailure)
            return Result<UserResponse>.Failure(updateResult.Errors);

        await repository.UpdateAsync(user, cancellationToken);
        await uow.CommitAsync();

        return new UserResponse(user.Id, user.UserName.Value, user.Role.ToString(), user.Active);
    }
}
