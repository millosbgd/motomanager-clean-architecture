namespace MotoManager.Domain.Entities;

public class ServiceOrderLabor
{
    public int Id { get; set; }
    public int ServiceOrderId { get; set; }
    public string OpisRadova { get; set; } = string.Empty;
    public decimal UkupnoVreme { get; set; }
    public decimal Cena { get; set; }

    // Navigation property
    public ServiceOrder ServiceOrder { get; set; } = null!;
}
