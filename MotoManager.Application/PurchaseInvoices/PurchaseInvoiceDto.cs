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
    decimal IznosBruto);

public record CreatePurchaseInvoiceRequest(
    string BrojRacuna, 
    DateTime Datum, 
    int DobavljacId, 
    int? VoziloId, 
    decimal IznosNeto, 
    decimal IznosPDV, 
    decimal IznosBruto);

public record UpdatePurchaseInvoiceRequest(
    int Id, 
    string BrojRacuna, 
    DateTime Datum, 
    int DobavljacId, 
    int? VoziloId, 
    decimal IznosNeto, 
    decimal IznosPDV, 
    decimal IznosBruto);
