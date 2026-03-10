using MassLab.Identity.Domain.Interfaces;
using MassLab.Identity.Domain.ValueObjects;
using MassLab.Shared.Errors;
using MassLab.Shared.Persistence;
using MassLab.Shared.Results;

namespace MassLab.Identity.Application.Commands.ChangePassword;

public class ChangePasswordHandler(IUserRepository repository, IUnitOfWork uow, IEncrypter encrypter)
{
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure(ErrorMessages.USER_NOT_FOUND);

        var passwordResult = Password.Create(request.NewPassword);
        if (passwordResult.IsFailure)
            return Result.Failure(passwordResult.Errors);

        var passwordHash = encrypter.Encrypt(request.NewPassword);
        var changeResult = user.ChangePassword(passwordHash);

        if (changeResult.IsFailure)
            return Result.Failure(changeResult.Errors);

        await repository.UpdateAsync(user, cancellationToken);
        await uow.CommitAsync();

        return Result.Success();
    }
}
