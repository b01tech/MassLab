using MassLab.Registry.Domain.Entities;
using MassLab.Registry.Domain.Enums;
using MassLab.Shared.ValueObject;

namespace MassLab.Registry.Domain.Tests.Entities;

public class ScaleTests
{
    [Fact]
    public void Create_ShouldReturnSuccess_WhenScaleIsValid()
    {
        var scaleResult = Scale.Create(
            "TAG-001",
            "Manufacturer X",
            "Model Y",
            "ID-123",
            100.0f,
            0.1f,
            "SN-999",
            ScaleClass.CLASS_III
        );

        Assert.True(scaleResult.IsSuccess);
        Assert.Equal("TAG-001", scaleResult.Data.Tag);
        Assert.Equal("Manufacturer X", scaleResult.Data.Manufacturer);
        Assert.Equal("Model Y", scaleResult.Data.Model);
        Assert.Equal("ID-123", scaleResult.Data.Identifier);
        Assert.Equal(100.0f, scaleResult.Data.CapMax);
        Assert.Equal(0.1f, scaleResult.Data.Resolution);
        Assert.Equal("SN-999", scaleResult.Data.SerialNumber);
        Assert.Equal(ScaleClass.CLASS_III, scaleResult.Data.Class);
        Assert.Equal(EquipmentType.Scale, scaleResult.Data.Type);
    }

    [Fact]
    public void Update_ShouldUpdatePropertiesAndSetUpdatedAt()
    {
        var scale = Scale.Create(
            "TAG-001",
            "Manufacturer X",
            "Model Y",
            "ID-123",
            100.0f,
            0.1f,
            "SN-999",
            ScaleClass.CLASS_III
        ).Data;

        scale.Update(
            "TAG-002",
            "Manufacturer Z",
            "Model W",
            "ID-456",
            200.0f,
            0.2f,
            "SN-888",
            ScaleClass.CLASS_II
        );

        Assert.Equal("TAG-002", scale.Tag);
        Assert.Equal("Manufacturer Z", scale.Manufacturer);
        Assert.Equal("Model W", scale.Model);
        Assert.Equal("ID-456", scale.Identifier);
        Assert.Equal(200.0f, scale.CapMax);
        Assert.Equal(0.2f, scale.Resolution);
        Assert.Equal("SN-888", scale.SerialNumber);
        Assert.Equal(ScaleClass.CLASS_II, scale.Class);
        Assert.NotNull(scale.UpdatedAt);
    }
}
