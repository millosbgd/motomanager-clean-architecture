namespace MotoManager.Application.Vehicles;

public record VehicleDto(
    int Id,
    string Model,
    string Plate,
    int ClientId,
    string ClientNaziv
);

public record CreateVehicleRequest(
    string Model,
    string Plate,
    int ClientId
);

public record UpdateVehicleRequest(
    int Id,
    string Model,
    string Plate,
    int ClientId
);
