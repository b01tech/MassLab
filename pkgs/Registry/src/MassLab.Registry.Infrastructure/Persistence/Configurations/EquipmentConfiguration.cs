using MassLab.Registry.Domain.Entities;
using MassLab.Registry.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace MassLab.Registry.Infrastructure.Persistence.Configurations;

public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.ToTable("Equipments");
        
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.Tag).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Manufacturer).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Model).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Identifier).HasMaxLength(50);
        
        builder.HasDiscriminator(e => e.Type)
            .HasValue<Scale>(EquipmentType.Scale);

        builder.Property(e => e.History)
            .HasField("_history")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
            )
            .Metadata.SetValueComparer(new ValueComparer<IEnumerable<string>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));
            
        builder.Property(e => e.History).HasColumnType("jsonb");
    }
}
