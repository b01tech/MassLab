using MassLab.Shared.Errors;
using MassLab.Shared.Results;

using System.Linq;

namespace MassLab.Shared.ValueObject;

public record Address
{
    public string Street { get; }
    public string Number { get; }
    public string Complement { get; }
    public string Neighborhood { get; }
    public string ZipCode { get; }
    public City City { get; }
    public State State { get; }

    private Address(string street, string number, string complement, string neighborhood, string zipCode, City city, State state)
    {
        Street = street;
        Number = number;
        Complement = complement;
        Neighborhood = neighborhood;
        ZipCode = zipCode;
        City = city;
        State = state;
    }

    public static Result<Address> Create(string street, string number, string complement, string neighborhood, string zipCode, string cityName, string stateCode)
    {
        if (string.IsNullOrWhiteSpace(street))
            return Result<Address>.Failure(ErrorMessages.NAME_INVALID);

        if (string.IsNullOrWhiteSpace(number))
            return Result<Address>.Failure(ErrorMessages.NAME_INVALID);

        if (string.IsNullOrWhiteSpace(neighborhood))
            return Result<Address>.Failure(ErrorMessages.NAME_INVALID);

        if (string.IsNullOrWhiteSpace(zipCode))
            return Result<Address>.Failure(ErrorMessages.ZIPCODE_INVALID);

        // Basic ZipCode validation (8 digits)
        var zipDigits = new string(zipCode.Where(char.IsDigit).ToArray());
        if (zipDigits.Length != 8)
            return Result<Address>.Failure(ErrorMessages.ZIPCODE_INVALID);

        var cityResult = City.Create(cityName);
        if (cityResult.IsFailure)
            return Result<Address>.Failure(cityResult.Errors);

        var stateResult = State.Create(stateCode);
        if (stateResult.IsFailure)
            return Result<Address>.Failure(stateResult.Errors);

        return new Address(street.Trim(), number.Trim(), complement?.Trim() ?? "", neighborhood.Trim(), zipDigits, cityResult.Data, stateResult.Data);
    }

    public string FormattedZipCode => Convert.ToUInt64(ZipCode).ToString(@"00000\-000");

    public override string ToString() => $"{Street}, {Number} - {Neighborhood}, {City}, {State} - {FormattedZipCode}";
}
