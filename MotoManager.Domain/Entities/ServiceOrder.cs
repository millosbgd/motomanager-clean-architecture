namespace MotoManager.Domain.Entities;

public class ServiceOrder
{
    public int Id { get; set; }
    public string BrojNaloga { get; set; } = string.Empty;
    public System.DateTime Datum { get; set; }
    public int ClientId { get; set; }
    public int VehicleId { get; set; }
    public string OpisRada { get; set; } = string.Empty;
    public int Kilometraza { get; set; }
    public string? KorisnikId { get; set; }

    // Navigation properties
    public Korisnik? Korisnik { get; set; }
    public Client Client { get; set; } = null!;
    public Vehicle Vehicle { get; set; } = null!;
    public System.Collections.Generic.ICollection<ServiceOrderLabor> Labors { get; set; } = new System.Collections.Generic.List<ServiceOrderLabor>();
    public System.Collections.Generic.ICollection<ServiceOrderMaterial> Materials { get; set; } = new System.Collections.Generic.List<ServiceOrderMaterial>();
}
