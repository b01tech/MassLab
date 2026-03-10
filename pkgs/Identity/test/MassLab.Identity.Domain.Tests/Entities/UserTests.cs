using MassLab.Identity.Domain.Entities;
using MassLab.Identity.Domain.Enums;
using MassLab.Shared.Errors;

namespace MassLab.Identity.Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void Create_ShouldReturnFailure_WhenNameIsInvalid()
    {
        var result = User.Create("", "valid_hash", "Operator");

        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.NAME_INVALID, result.Errors);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenHashIsInvalid()
    {
        var result = User.Create("Valid Name", "", "Operator");

        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.HASH_INVALID, result.Errors);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenRoleIsInvalid()
    {
        var result = User.Create("Valid Name", "valid_hash", "InvalidRole");

        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.ROLE_INVALID, result.Errors);
    }

    [Fact]
    public void Create_ShouldReturnSuccess_WhenDataIsValid()
    {
        var name = "Valid Name";
        var hash = "valid_hash";
        var role = "Admin";

        var result = User.Create(name, hash, role);

        Assert.True(result.IsSuccess);
        Assert.Equal(name, result.Data.UserName.Value);
        Assert.Equal(hash, result.Data.HashPassword.Value);
        Assert.Equal(UserRole.Admin, result.Data.Role);
        Assert.True(result.Data.Active);
    }

    [Fact]
    public void Deactivate_ShouldSetActiveToFalse()
    {
        var user = User.Create("Valid Name", "valid_hash", "Operator").Data;

        user.Deactivate();

        Assert.False(user.Active);
        Assert.NotNull(user.UpdatedAt);
    }

    [Fact]
    public void Activate_ShouldSetActiveToTrue()
    {
        var user = User.Create("Valid Name", "valid_hash", "Operator").Data;
        user.Deactivate();

        user.Activate();

        Assert.True(user.Active);
        Assert.NotNull(user.UpdatedAt);
    }

    [Fact]
    public void Update_ShouldUpdateNameAndRole()
    {
        var user = User.Create("Valid Name", "valid_hash", "Operator").Data;
        var newName = "New Name";
        var newRole = "Manager";

        var result = user.Update(newName, newRole);

        Assert.True(result.IsSuccess);
        Assert.Equal(newName, user.UserName.Value);
        Assert.Equal(UserRole.Manager, user.Role);
        Assert.NotNull(user.UpdatedAt);
    }

    [Fact]
    public void Update_ShouldReturnFailure_WhenRoleIsInvalid()
    {
        var user = User.Create("Valid Name", "valid_hash", "Operator").Data;
        var newName = "New Name";
        var newRole = "InvalidRole";

        var result = user.Update(newName, newRole);

        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessages.ROLE_INVALID, result.Errors);
    }

    [Fact]
    public void ChangePassword_ShouldUpdatePasswordHash()
    {
        var user = User.Create("Valid Name", "valid_hash", "Operator").Data;
        var newHash = "new_valid_hash";

        var result = user.ChangePassword(newHash);

        Assert.True(result.IsSuccess);
        Assert.Equal(newHash, user.HashPassword.Value);
        Assert.NotNull(user.UpdatedAt);
    }
}
