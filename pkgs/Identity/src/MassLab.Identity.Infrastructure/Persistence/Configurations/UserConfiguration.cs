using MassLab.Identity.Domain.Entities;
using MassLab.Shared.ValueObject;
using MassLab.Identity.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MassLab.Identity.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .ValueGeneratedNever();

        builder.OwnsOne(u => u.UserName, name =>
        {
            name.Property(n => n.Value)
                .HasColumnName("UserName")
                .HasMaxLength(255)
                .IsRequired();

            name.HasIndex(n => n.Value).IsUnique();
        });

        builder.OwnsOne(u => u.HashPassword, hash =>
        {
            hash.Property(h => h.Value)
                .HasColumnName("PasswordHash")
                .IsRequired();
        });

        builder.Property(u => u.Role)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.Active)
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.UpdatedAt);
    }
}
