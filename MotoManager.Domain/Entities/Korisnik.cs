namespace MotoManager.Domain.Entities;

public class Korisnik
{
    public string Id { get; set; } = string.Empty; // Auth0 User ID
    public string ImePrezime { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public int SektorId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EditedAt { get; set; }
    
    // Navigation property
    public Sektor? Sektor { get; set; }
}
