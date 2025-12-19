namespace MotoManager.Application.Sektori;

public class SektorDto
{
    public int Id { get; set; }
    public string Naziv { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? EditedAt { get; set; }
}

public class CreateSektorRequest
{
    public string Naziv { get; set; } = string.Empty;
}

public class UpdateSektorRequest
{
    public int Id { get; set; }
    public string Naziv { get; set; } = string.Empty;
}
