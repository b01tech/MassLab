using MassLab.Registry.Domain.Enums;
using MassLab.Shared.Results;

namespace MassLab.Registry.Domain.Entities;

public class Scale : Equipment
{
    public float CapMax { get; private set; }
    public float Resolution { get; private set; }
    public string SerialNumber { get; private set; } = string.Empty;
    public ScaleClass Class { get; private set; }
    // Remove static property or keep if user insists, but EF uses Type from base.
    // public static new EquipmentType Type => EquipmentType.Scale; 

    private Scale() { } // EF

    private Scale(string tag, string manufacturer, string model, string? identifier, float capMax, float resolution, string serialNumber, ScaleClass scaleClass) 
        : base(tag, manufacturer, model, identifier, EquipmentType.Scale)
    {
        CapMax = capMax;
        Resolution = resolution;
        SerialNumber = serialNumber;
        Class = scaleClass;
    }

    public static Result<Scale> Create(string tag, string manufacturer, string model, string? identifier, float capMax, float resolution, string serialNumber, ScaleClass scaleClass)
    {
        // Add validations here if needed
        return new Scale(tag, manufacturer, model, identifier, capMax, resolution, serialNumber, scaleClass);
    }

    public void Update(string tag, string manufacturer, string model, string? identifier, float capMax, float resolution, string serialNumber, ScaleClass scaleClass)
    {
        base.Update(tag, manufacturer, model, identifier);
        CapMax = capMax;
        Resolution = resolution;
        SerialNumber = serialNumber;
        Class = scaleClass;
    }
}
