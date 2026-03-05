using MassLib.Identity.Domain.Interfaces;
using MassLib.Shared.Errors;
using MassLib.Shared.Persistence;
using MassLib.Shared.Results;

namespace MassLib.Identity.Application.Commands.ActivateUser;

public class ActivateUserHandler(IUserRepository repository, IUnitOfWork uow)
{
    public async Task<Result> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure(ErrorMessages.USER_NOT_FOUND);

        user.Activate();

        await repository.UpdateAsync(user, cancellationToken);
        await uow.CommitAsync();

        return Result.Success();
    }
}
