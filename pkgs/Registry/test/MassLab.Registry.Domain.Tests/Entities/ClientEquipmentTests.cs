using MassLab.Registry.Domain.Entities;
using MassLab.Registry.Domain.Enums;
using MassLab.Shared.ValueObject;

namespace MassLab.Registry.Domain.Tests.Entities;

public class ClientEquipmentTests
{
    [Fact]
    public void AddEquipment_ShouldAddEquipmentAndSetUpdatedAt()
    {
        var client = Client.Create(
            Name.Create("Client Ltda").Data,
            Name.Create("Client").Data,
            Cnpj.Create("12345678000195").Data
        ).Data;

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

        client.AddEquipment(scale);

        Assert.Single(client.Equipments);
        Assert.Equal(scale, client.Equipments.First());
        Assert.NotNull(client.UpdatedAt);
    }

    [Fact]
    public void RemoveEquipment_ShouldRemoveEquipmentAndSetUpdatedAt()
    {
        var client = Client.Create(
            Name.Create("Client Ltda").Data,
            Name.Create("Client").Data,
            Cnpj.Create("12345678000195").Data
        ).Data;

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

        client.AddEquipment(scale);
        client.RemoveEquipment(scale);

        Assert.Empty(client.Equipments);
    }
}
