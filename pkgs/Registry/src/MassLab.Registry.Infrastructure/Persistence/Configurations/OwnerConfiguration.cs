using MassLab.Registry.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MassLab.Registry.Infrastructure.Persistence.Configurations;

public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder.ToTable("Owners");

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).ValueGeneratedNever();

        builder.OwnsOne(o => o.LegalName, name =>
        {
            name.Property(n => n.Value).HasColumnName("LegalName").HasMaxLength(255).IsRequired();
        });

        builder.OwnsOne(o => o.BusinessName, name =>
        {
            name.Property(n => n.Value).HasColumnName("BusinessName").HasMaxLength(255).IsRequired();
        });

        builder.OwnsOne(o => o.Cnpj, cnpj =>
        {
            cnpj.Property(c => c.Value).HasColumnName("Cnpj").HasMaxLength(14).IsRequired();
        });

        builder.OwnsOne(o => o.Address, address =>
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

        builder.OwnsOne(o => o.Email, email =>
        {
            email.Property(e => e.Value).HasColumnName("Email").HasMaxLength(255);
        });

        builder.OwnsOne(o => o.Phone, phone =>
        {
            phone.Property(p => p.Value).HasColumnName("Phone").HasMaxLength(20);
        });

        builder.Property(o => o.LogoUrl).HasMaxLength(500);
    }
}
