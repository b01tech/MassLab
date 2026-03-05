using MassLib.Identity.Domain.Enums;
using MassLib.Identity.Domain.ValueObjects;
using MassLib.Shared.Entities;
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

    public static Result<User> Create(string name, string hash, UserRole role)
    {
        var nameResult = Name.Create(name);
        var hashResult = Hash.Create(hash);

        if (nameResult.IsFailure || hashResult.IsFailure)
            return Result<User>.Failure(Result.MergeErrors(nameResult, hashResult));

        return new User
        {
            UserName = nameResult.Data,
            HashPassword = hashResult.Data,
            Role = role
        };
    }

    public Result Update(string name, UserRole role)
    {
        var nameResult = Name.Create(name);
        if (nameResult.IsFailure)
            return nameResult;

        UserName = nameResult.Data;
        Role = role;
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
}
