namespace MotoManager.Application.Materials;

public record MaterialDto(
    int Id,
    string Naziv,
    decimal JedinicnaCena);

public record CreateMaterialRequest(
    string Naziv,
    decimal JedinicnaCena);

public record UpdateMaterialRequest(
    int Id,
    string Naziv,
    decimal JedinicnaCena);
