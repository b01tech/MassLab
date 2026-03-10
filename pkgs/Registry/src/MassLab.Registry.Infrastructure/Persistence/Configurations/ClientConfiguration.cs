using MassLab.Registry.Domain.Entities;
using MassLab.Shared.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MassLab.Registry.Infrastructure.Persistence.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.OwnsOne(c => c.LegalName, name =>
        {
            name.Property(n => n.Value).HasColumnName("LegalName").HasMaxLength(255).IsRequired();
        });

        builder.OwnsOne(c => c.BusinessName, name =>
        {
            name.Property(n => n.Value).HasColumnName("BusinessName").HasMaxLength(255); // Pode ser nulo no Client? No código estava Required, mas mudei para nullable na entidade.
        });

        // Configuração customizada para DocumentNumber polimórfico
        builder.Property(c => c.DocumentNumber)
            .HasConversion(
                v => v != null ? v.Value : null,
                v => v != null ? CreateDocumentNumber(v) : null
            )
            .HasColumnName("DocumentNumber")
            .HasMaxLength(14);

        builder.OwnsOne(c => c.Address, address =>
        {
            address.Property(a => a.Street).HasColumnName("Address_Street").HasMaxLength(255);
            address.Property(a => a.Number).HasColumnName("Address_Number").HasMaxLength(50);
            address.Property(a => a.Complement).HasColumnName("Address_Complement").HasMaxLength(100);
            address.Property(a => a.Neighborhood).HasColumnName("Address_Neighborhood").HasMaxLength(100);
            address.Property(a => a.ZipCode).HasColumnName("Address_ZipCode").HasMaxLength(8);
            
            address.OwnsOne(a => a.City, city =>
            {
                city.Property(c => c.Name).HasColumnName("Address_City").HasMaxLength(100);
            });

            address.OwnsOne(a => a.State, state =>
            {
                state.Property(s => s.Value).HasColumnName("Address_State").HasMaxLength(2);
            });
        });

        builder.OwnsOne(c => c.Email, email =>
        {
            email.Property(e => e.Value).HasColumnName("Email").HasMaxLength(255);
        });

        builder.OwnsOne(c => c.Phone, phone =>
        {
            phone.Property(p => p.Value).HasColumnName("Phone").HasMaxLength(20);
        });

        // Configuração da coleção de contatos
        // Owned Collection mapeada para outra tabela
        builder.OwnsMany(c => c.Contacts, contact =>
        {
            contact.ToTable("ClientContacts");
            contact.WithOwner().HasForeignKey("ClientId");
            contact.HasKey("ClientId", "Email", "Phone"); // Chave composta ou Id sintético?
            // Melhor usar Id sintético implícito do EF ou configurar propriedades
            
            contact.OwnsOne(cc => cc.Name, name =>
            {
                name.Property(n => n.Value).HasColumnName("Name").HasMaxLength(255).IsRequired();
            });

            contact.OwnsOne(cc => cc.Email, email =>
            {
                email.Property(e => e.Value).HasColumnName("Email").HasMaxLength(255).IsRequired();
            });
            
            contact.OwnsOne(cc => cc.Phone, phone =>
            {
                phone.Property(p => p.Value).HasColumnName("Phone").HasMaxLength(20).IsRequired();
            });
        });
    }

    private static DocumentNumber CreateDocumentNumber(string value)
    {
        if (value.Length == 11)
            return Cpf.Create(value).Data; // Assumindo dados válidos no banco
        if (value.Length == 14)
            return Cnpj.Create(value).Data;
        
        throw new InvalidOperationException($"Invalid document number length: {value.Length}");
    }
}
