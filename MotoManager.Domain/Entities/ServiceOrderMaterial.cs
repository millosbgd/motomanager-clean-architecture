namespace MotoManager.Domain.Entities;

public class ServiceOrderMaterial
{
    public int Id { get; set; }
    public int ServiceOrderId { get; set; }
    public int MaterialId { get; set; }
    public decimal Kolicina { get; set; }
    public decimal JedinicnaCena { get; set; }
    public decimal UkupnaCena { get; set; }

    // Navigation properties
    public ServiceOrder ServiceOrder { get; set; } = null!;
    public Material Material { get; set; } = null!;
}
