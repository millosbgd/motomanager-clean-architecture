namespace MotoManager.Application.Korisnici;

public class KorisnikDto
{
    public string Id { get; set; } = string.Empty;
    public string ImePrezime { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public int SektorId { get; set; }
    public string SektorNaziv { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? EditedAt { get; set; }
}

public class CreateKorisnikRequest
{
    public string Id { get; set; } = string.Empty; // Auth0 User ID
    public string ImePrezime { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public int SektorId { get; set; }
}

public class UpdateKorisnikRequest
{
    public string Id { get; set; } = string.Empty;
    public string ImePrezime { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public int SektorId { get; set; }
}
