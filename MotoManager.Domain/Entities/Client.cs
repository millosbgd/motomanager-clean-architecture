namespace MotoManager.Domain.Entities;

public class Client
{
    public int Id { get; set; }
    public string Naziv { get; set; } = string.Empty;
    public string Adresa { get; set; } = string.Empty;
    public string Grad { get; set; } = string.Empty;
    public string? PIB { get; set; }
    public string Telefon { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
