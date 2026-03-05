using System.Text.RegularExpressions;
using MassLib.Shared.Errors;
using MassLib.Shared.Results;

namespace MassLib.Identity.Domain.ValueObjects;

public record Password
{
    public string Value { get; }
    private const int MinLength = 6;
    private const string RegexPattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*?&]{6,}$";

    private Password(string value)
    {
        Value = value;
    }

    public static Result<Password> Create(string password)
    {
        var formatedPassword = password.Trim();

        if (!Regex.IsMatch(formatedPassword, RegexPattern))
            return Result<Password>.Failure(ErrorMessages.PASSWORD_INVALID);

        return new Password(formatedPassword);
    }

    public override string ToString() => Value;
};
