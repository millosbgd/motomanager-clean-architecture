namespace MotoManager.Domain.Entities;

public class Sektor
{
    public int Id { get; set; }
    public string Naziv { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EditedAt { get; set; }
}
