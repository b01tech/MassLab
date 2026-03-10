using MassLab.Registry.Domain.Entities;
using MassLab.Shared.ValueObject;
using System.Linq;

namespace MassLab.Registry.Domain.Tests.Entities;

public class OwnerTests
{
    [Fact]
    public void Create_ShouldReturnSuccess_WhenOwnerIsValid()
    {
        var legalName = Name.Create("MassLab Ltda").Data;
        var businessName = Name.Create("MassLab").Data;
        var cnpj = Cnpj.Create("12345678000195").Data; // Valid CNPJ
        
        var ownerResult = Owner.Create(legalName, businessName, cnpj);

        Assert.True(ownerResult.IsSuccess);
        Assert.Equal(legalName, ownerResult.Data.LegalName);
        Assert.Equal(businessName, ownerResult.Data.BusinessName);
        Assert.Equal(cnpj, ownerResult.Data.Cnpj);
    }

    [Fact]
    public void Update_ShouldUpdatePropertiesAndSetUpdatedAt()
    {
        var legalName = Name.Create("MassLab Ltda").Data;
        var businessName = Name.Create("MassLab").Data;
        var cnpj = Cnpj.Create("12345678000195").Data;
        var owner = Owner.Create(legalName, businessName, cnpj).Data;

        var newLegalName = Name.Create("MassLab New Ltda").Data;
        var newBusinessName = Name.Create("MassLab New").Data;
        
        owner.Update(newLegalName, newBusinessName, null, null, null, null);

        Assert.Equal(newLegalName, owner.LegalName);
        Assert.Equal(newBusinessName, owner.BusinessName);
        Assert.NotNull(owner.UpdatedAt);
    }
}
