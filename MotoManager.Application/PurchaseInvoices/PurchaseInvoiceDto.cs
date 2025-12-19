namespace MotoManager.Application.PurchaseInvoices;

public record PurchaseInvoiceDto(
    int Id, 
    string BrojRacuna, 
    DateTime Datum, 
    int DobavljacId, 
    string DobavljacNaziv,
    int? VoziloId, 
    string? VoziloModel,
    string? VoziloPlate,
    decimal IznosNeto, 
    decimal IznosPDV, 
    decimal IznosBruto,
    string? KorisnikId,
    string? KorisnikImePrezime,
    int? SektorId,
    string? SektorNaziv);

public record CreatePurchaseInvoiceRequest(
    string BrojRacuna, 
    DateTime Datum, 
    int DobavljacId, 
    int? VoziloId, 
    decimal IznosNeto, 
    decimal IznosPDV, 
    decimal IznosBruto,
    string? KorisnikId,
    int? SektorId);

public record UpdatePurchaseInvoiceRequest(
    int Id, 
    string BrojRacuna, 
    DateTime Datum, 
    int DobavljacId, 
    int? VoziloId, 
    decimal IznosNeto, 
    decimal IznosPDV, 
    decimal IznosBruto,
    string? KorisnikId,
    int? SektorId);
