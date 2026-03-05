using MassLib.Identity.Domain.Enums;
using MassLib.Identity.Domain.ValueObjects;
using MassLib.Shared.Entities;
using MassLib.Shared.Errors;
using MassLib.Shared.Results;
using MassLib.Shared.ValueObject;

namespace MassLib.Identity.Domain.Entities;

public class User : Entity
{
    public Name UserName { get; private set; }
    public Hash HashPassword { get; private set; }
    public UserRole Role { get; private set; }
    public bool Active { get; private set; } = true;

    private User()
        : base() { }

    public static Result<User> Create(string name, string hash, string role)
    {
        var nameResult = Name.Create(name);
        var hashResult = Hash.Create(hash);
        var userRoleResult = ValidateUserRole(role);

        if (nameResult.IsFailure || hashResult.IsFailure || userRoleResult.IsFailure)
            return Result<User>.Failure(Result.MergeErrors(nameResult, hashResult, userRoleResult));

        return new User
        {
            UserName = nameResult.Data,
            HashPassword = hashResult.Data,
            Role = userRoleResult.Data
        };
    }

    public Result Update(string name, string role)
    {
        var nameResult = Name.Create(name);
        var roleResult = ValidateUserRole(role);

        if (nameResult.IsFailure || roleResult.IsFailure)
            return Result.Failure(Result.MergeErrors(nameResult, roleResult));

        UserName = nameResult.Data;
        Role = roleResult.Data;
        SetUpdatedAt();

        return Result.Success();
    }

    public void Deactivate()
    {
        Active = false;
        SetUpdatedAt();
    }

    public void Activate()
    {
        Active = true;
        SetUpdatedAt();
    }

    public Result ChangePassword(string newPasswordHash)
    {
        var hashResult = Hash.Create(newPasswordHash);
        if (hashResult.IsFailure)
            return hashResult;

        HashPassword = hashResult.Data;
        SetUpdatedAt();
        return Result.Success();
    }

    private static Result<UserRole> ValidateUserRole(string userRole)
    {
        if (Enum.TryParse<UserRole>(userRole, true, out var role) && Enum.IsDefined(typeof(UserRole), role))
            return Result<UserRole>.Success(role);

        return Result<UserRole>.Failure(ErrorMessages.ROLE_INVALID);
    }
}
