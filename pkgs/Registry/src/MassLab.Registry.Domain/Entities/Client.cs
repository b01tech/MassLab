using System.Linq;
using System.Collections.Generic;
using MassLab.Shared.Entities;
using MassLab.Shared.ValueObject;
using MassLab.Shared.Results;

namespace MassLab.Registry.Domain.Entities;

public class Client : Entity
{
    public Name LegalName { get; private set; }
    public Name? BusinessName { get; private set; }
    public DocumentNumber? DocumentNumber { get; private set; }
    public Address? Address { get; private set; }
    public Email? Email { get; private set; }
    public Phone? Phone { get; private set; }
    private readonly List<Contact> _contacts = new();
    public IReadOnlyCollection<Contact> Contacts => _contacts.AsReadOnly();

    protected Client()
    {
        LegalName = null!;
        BusinessName = null!;
        DocumentNumber = null!;
    } // EF

    private Client(Name legalName, Name? businessName, DocumentNumber? documentNumber, Address? address, Email? email, Phone? phone)
    {
        LegalName = legalName;
        BusinessName = businessName;
        DocumentNumber = documentNumber;
        Address = address;
        Email = email;
        Phone = phone;
    }

    public static Result<Client> Create(Name legalName, Name? businessName = null, DocumentNumber? documentNumber = null, Address? address = null, Email? email = null, Phone? phone = null)
    {
        return new Client(legalName, businessName, documentNumber, address, email, phone);
    }

    public void Update(Name legalName, Name businessName, Address? address, Email? email, Phone? phone)
    {
        LegalName = legalName;
        BusinessName = businessName;
        Address = address;
        Email = email;
        Phone = phone;
        SetUpdatedAt();
    }

    public void AddContact(Contact contact)
    {
        if (!_contacts.Any(c => c.Email == contact.Email && c.Phone == contact.Phone))
        {
            _contacts.Add(contact);
            SetUpdatedAt();
        }
    }

    public void RemoveContact(Contact contact)
    {
        var existing = _contacts.FirstOrDefault(c => c.Email == contact.Email);
        if (existing != null)
        {
            _contacts.Remove(existing);
            SetUpdatedAt();
        }
    }
}
