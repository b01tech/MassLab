using MassLab.Shared.Entities;
using MassLab.Shared.ValueObject;
using MassLab.Shared.Results;

namespace MassLab.Registry.Domain.Entities;

public class Owner : Entity
{
    public Name LegalName { get; private set; }
    public Name BusinessName { get; private set; }
    public Cnpj Cnpj { get; private set; }
    public Address? Address { get; private set; }
    public Email? Email { get; private set; }
    public Phone? Phone { get; private set; }
    public string? LogoUrl { get; private set; }

    protected Owner()
    {
        LegalName = null!;
        BusinessName = null!;
        Cnpj = null!;
    } // EF

    private Owner(Name legalName, Name businessName, Cnpj cnpj, Address? address, Email? email, Phone? phone, string? logoUrl)
    {
        LegalName = legalName;
        BusinessName = businessName;
        Cnpj = cnpj;
        Address = address;
        Email = email;
        Phone = phone;
        LogoUrl = logoUrl;
    }

    public static Result<Owner> Create(Name legalName, Name businessName, Cnpj cnpj, Address? address = null, Email? email = null, Phone? phone = null, string? logoUrl = null)
    {
        return new Owner(legalName, businessName, cnpj, address, email, phone, logoUrl);
    }

    public void Update(Name legalName, Name businessName, Address? address, Email? email, Phone? phone, string? logoUrl)
    {
        LegalName = legalName;
        BusinessName = businessName;
        Address = address;
        Email = email;
        Phone = phone;
        LogoUrl = logoUrl;
        SetUpdatedAt();
    }
}
