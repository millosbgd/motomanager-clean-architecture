namespace MotoManager.Application.ServiceOrderMaterials;

public record ServiceOrderMaterialDto(
    int Id,
    int ServiceOrderId,
    int MaterialId,
    string MaterialNaziv,
    decimal Kolicina,
    decimal JedinicnaCena,
    decimal UkupnaCena);

public record CreateServiceOrderMaterialRequest(
    int ServiceOrderId,
    int MaterialId,
    decimal Kolicina,
    decimal JedinicnaCena,
    decimal UkupnaCena);

public record UpdateServiceOrderMaterialRequest(
    int Id,
    int ServiceOrderId,
    int MaterialId,
    decimal Kolicina,
    decimal JedinicnaCena,
    decimal UkupnaCena);
