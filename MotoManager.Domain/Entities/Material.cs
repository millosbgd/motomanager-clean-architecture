namespace MotoManager.Domain.Entities;

public class Material
{
    public int Id { get; set; }
    public string Naziv { get; set; } = string.Empty;
    public decimal JedinicnaCena { get; set; }
}
