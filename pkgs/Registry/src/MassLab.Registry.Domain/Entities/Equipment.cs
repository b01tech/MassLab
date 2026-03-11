using MassLab.Registry.Domain.Enums;
using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.Entities;

namespace MassLab.Registry.Domain.Entities;

public abstract class Equipment : Entity, IEquipment
{
    public string Tag { get; private set; } = string.Empty;
    public string Manufacturer { get; private set; } = string.Empty;
    public string Model { get; private set; } = string.Empty;
    public EquipmentType Type { get; private set; }
    public string? Identifier { get; private set; }
    private readonly List<string> _history = new();
    public IEnumerable<string> History => _history.AsReadOnly();

    protected Equipment() { } // EF

    protected Equipment(string tag, string manufacturer, string model, string? identifier, EquipmentType type)
    {
        Tag = tag;
        Manufacturer = manufacturer;
        Model = model;
        Identifier = identifier;
        Type = type;
    }

    public void AddHistory(string note)
    {
        _history.Add(note);
        SetUpdatedAt();
    }
    
    // Updates common fields
    public void Update(string tag, string manufacturer, string model, string? identifier)
    {
        Tag = tag;
        Manufacturer = manufacturer;
        Model = model;
        Identifier = identifier;
        SetUpdatedAt();
    }
}
