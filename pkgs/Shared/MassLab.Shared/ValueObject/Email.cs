using System.Text.RegularExpressions;
using MassLab.Shared.Errors;
using MassLab.Shared.Results;

namespace MassLab.Shared.ValueObject;

public record Email
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result<Email>.Failure(ErrorMessages.EMAIL_INVALID);

        // Simple regex or MailAddress check
        var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        if (!regex.IsMatch(email))
            return Result<Email>.Failure(ErrorMessages.EMAIL_INVALID);

        return new Email(email);
    }

    public override string ToString() => Value;
};
