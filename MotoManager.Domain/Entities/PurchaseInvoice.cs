namespace MotoManager.Domain.Entities;

public class PurchaseInvoice
{
    public int Id { get; set; }
    public string BrojRacuna { get; set; } = string.Empty;
    public DateTime Datum { get; set; }
    public int DobavljacId { get; set; }
    public Client Dobavljac { get; set; } = null!;
    public int? VoziloId { get; set; }
    public Vehicle? Vozilo { get; set; }
    public decimal IznosNeto { get; set; }
    public decimal IznosPDV { get; set; }
    public decimal IznosBruto { get; set; }
    public string? KorisnikId { get; set; }
    public Korisnik? Korisnik { get; set; }
}
