using MassLab.Registry.Domain.Enums;

namespace MassLab.Registry.Domain.Interfaces;

public interface IEquipment
{
    string Tag { get; }
    string Manufacturer { get; }
    string Model { get; }
    EquipmentType Type { get; }
    string? Identifier { get; }
    IEnumerable<string> History { get; }
}
