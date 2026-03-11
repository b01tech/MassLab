using MassLab.Registry.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MassLab.Registry.Infrastructure.Persistence.Configurations;

public class ScaleConfiguration : IEntityTypeConfiguration<Scale>
{
    public void Configure(EntityTypeBuilder<Scale> builder)
    {
        builder.Property(s => s.CapMax).IsRequired();
        builder.Property(s => s.Resolution).IsRequired();
        builder.Property(s => s.SerialNumber).HasMaxLength(50).IsRequired();
        builder.Property(s => s.Class).IsRequired();
    }
}
