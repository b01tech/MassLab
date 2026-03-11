using MassLab.Shared.Errors;
using MassLab.Shared.Results;

namespace MassLab.Shared.ValueObject;

public record Contact
{
    public Name Name { get; private set; }
    public Email Email { get; private set; }
    public Phone Phone { get; private set; }

    private Contact()
    {
        Name = null!;
        Email = null!;
        Phone = null!;
    }

    private Contact(Name name, Email email, Phone phone)
    {
        Name = name;
        Email = email;
        Phone = phone;
    }

    public static Result<Contact> Create(string name, string email, string phone)
    {
        var nameResult = Name.Create(name);
        if (nameResult.IsFailure)
            return Result<Contact>.Failure(nameResult.Errors);

        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
            return Result<Contact>.Failure(emailResult.Errors);

        var phoneResult = Phone.Create(phone);
        if (phoneResult.IsFailure)
            return Result<Contact>.Failure(phoneResult.Errors);

        return new Contact(nameResult.Data, emailResult.Data, phoneResult.Data);
    }
}
