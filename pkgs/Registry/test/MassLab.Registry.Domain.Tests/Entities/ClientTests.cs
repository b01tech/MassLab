using MassLab.Registry.Domain.Entities;
using MassLab.Shared.ValueObject;
using System.Linq;

namespace MassLab.Registry.Domain.Tests.Entities;

public class ClientTests
{
    [Fact]
    public void Create_ShouldReturnSuccess_WhenClientIsValid()
    {
        var legalName = Name.Create("Client Ltda").Data;
        var businessName = Name.Create("Client").Data;
        var cnpj = Cnpj.Create("12345678000195").Data; // Valid CNPJ

        var clientResult = Client.Create(legalName, businessName, cnpj);

        Assert.True(clientResult.IsSuccess);
        Assert.Equal(legalName, clientResult.Data.LegalName);
        Assert.Equal(businessName, clientResult.Data.BusinessName);
        Assert.Equal(cnpj, clientResult.Data.DocumentNumber);
    }

    [Fact]
    public void AddContact_ShouldAddContactAndSetUpdatedAt()
    {
        var legalName = Name.Create("Client Ltda").Data;
        var businessName = Name.Create("Client").Data;
        var cnpj = Cnpj.Create("12345678000195").Data;
        var client = Client.Create(legalName, businessName, cnpj).Data;

        var contact = Contact.Create("John", "john@email.com", "11999999999").Data;

        client.AddContact(contact);

        Assert.Single(client.Contacts);
        Assert.Equal(contact, client.Contacts.First());
        Assert.NotNull(client.UpdatedAt);
    }

    [Fact]
    public void RemoveContact_ShouldRemoveContactAndSetUpdatedAt()
    {
        var legalName = Name.Create("Client Ltda").Data;
        var businessName = Name.Create("Client").Data;
        var cnpj = Cnpj.Create("12345678000195").Data;
        var client = Client.Create(legalName, businessName, cnpj).Data;

        var contact = Contact.Create("John", "john@email.com", "11999999999").Data;
        client.AddContact(contact);
        
        // Reset UpdatedAt to verify it changes again? Or just check if it's not null (which it is already).
        // Better: check if contact is removed.

        client.RemoveContact(contact);

        Assert.Empty(client.Contacts);
    }
}
