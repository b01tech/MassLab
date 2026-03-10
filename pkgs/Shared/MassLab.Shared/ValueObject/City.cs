using MassLab.Shared.Errors;
using MassLab.Shared.Results;

namespace MassLab.Shared.ValueObject;

public record City
{
    public string Name { get; }

    private City(string name)
    {
        Name = name;
    }

    public static Result<City> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<City>.Failure(ErrorMessages.NAME_INVALID);

        return new City(name.Trim());
    }

    public override string ToString() => Name;
}
