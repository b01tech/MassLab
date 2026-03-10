namespace MassLab.Registry.Application.DTOs;

public record AddressResponse(
    string Street,
    string Number,
    string Complement,
    string Neighborhood,
    string ZipCode,
    string City,
    string State
);
