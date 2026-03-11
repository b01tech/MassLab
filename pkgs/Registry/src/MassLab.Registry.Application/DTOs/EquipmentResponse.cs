using MassLab.Registry.Domain.Enums;
using System.Text.Json.Serialization;

namespace MassLab.Registry.Application.DTOs;

[JsonDerivedType(typeof(ScaleResponse), typeDiscriminator: "scale")]
public record EquipmentResponse(
    Guid Id,
    string Tag,
    string Manufacturer,
    string Model,
    EquipmentType Type,
    string? Identifier,
    IEnumerable<string> History
);

public record ScaleResponse(
    Guid Id,
    string Tag,
    string Manufacturer,
    string Model,
    string? Identifier,
    IEnumerable<string> History,
    float CapMax,
    float Resolution,
    string SerialNumber,
    ScaleClass Class
) : EquipmentResponse(Id, Tag, Manufacturer, Model, EquipmentType.Scale, Identifier, History);
