using MassLib.Identity.Application.DTOs;
using MassLib.Identity.Domain.Entities;
using MassLib.Identity.Domain.Interfaces;
using MassLib.Identity.Domain.ValueObjects;
using MassLib.Shared.Errors;
using MassLib.Shared.Persistence;
using MassLib.Shared.Results;

namespace MassLib.Identity.Application.Commands.CreateUser;

public class CreateUserHandler(IUserRepository repository, IUnitOfWork uow, IEncrypter encrypter)
{

    public async Task<Result<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await repository.GetByUserNameAsync(request.UserName, cancellationToken);
        if (existingUser is not null)
            return Result<UserResponse>.Failure(ErrorMessages.USERNAME_ALREADY_REGISTERED);

        var passwordResult = Password.Create(request.Password);
        if (passwordResult.IsFailure)
             return Result<UserResponse>.Failure(passwordResult.Errors);

        var passwordHash = encrypter.Encrypt(request.Password);
        var userResult = User.Create(request.UserName, passwordHash, request.Role);
        if (userResult.IsFailure)
            return Result<UserResponse>.Failure(userResult.Errors);

        var user = userResult.Data;
        await repository.AddAsync(user, cancellationToken);
        await uow.CommitAsync();

        return new UserResponse(user.Id, user.UserName.Value, user.Role, user.Active);
    }
}

