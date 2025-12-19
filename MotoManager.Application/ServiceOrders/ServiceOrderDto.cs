namespace MotoManager.Application.ServiceOrders;

public record ServiceOrderDto(
    int Id, 
    string BrojNaloga, 
    System.DateTime Datum, 
    int ClientId, 
    string ClientNaziv,
    int VehicleId, 
    string VehicleModel,
    string VehiclePlate,
    string OpisRada, 
    int Kilometraza,
    string? KorisnikId,
    string? KorisnikImePrezime);

public record CreateServiceOrderRequest(
    string BrojNaloga, 
    System.DateTime Datum, 
    int ClientId, 
    int VehicleId, 
    string OpisRada, 
    int Kilometraza,
    string? KorisnikId);

public record UpdateServiceOrderRequest(
    int Id,
    string BrojNaloga, 
    System.DateTime Datum, 
    int ClientId, 
    int VehicleId, 
    string OpisRada, 
    int Kilometraza,
    string? KorisnikId);
