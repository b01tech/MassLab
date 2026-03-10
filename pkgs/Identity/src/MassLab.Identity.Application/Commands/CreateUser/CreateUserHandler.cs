using MassLab.Identity.Application.DTOs;
using MassLab.Identity.Domain.Entities;
using MassLab.Identity.Domain.Enums;
using MassLab.Identity.Domain.Interfaces;
using MassLab.Identity.Domain.ValueObjects;
using MassLab.Shared.Errors;
using MassLab.Shared.Persistence;
using MassLab.Shared.Results;

namespace MassLab.Identity.Application.Commands.CreateUser;

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

        return new UserResponse(user.Id, user.UserName.Value, user.Role.ToString(), user.Active);
    }
}
