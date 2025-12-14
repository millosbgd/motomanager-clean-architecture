namespace MotoManager.Application.ServiceOrderLabors;

public record ServiceOrderLaborDto(
    int Id,
    int ServiceOrderId,
    string OpisRadova,
    decimal UkupnoVreme,
    decimal Cena);

public record CreateServiceOrderLaborRequest(
    int ServiceOrderId,
    string OpisRadova,
    decimal UkupnoVreme,
    decimal Cena);

public record UpdateServiceOrderLaborRequest(
    int Id,
    int ServiceOrderId,
    string OpisRadova,
    decimal UkupnoVreme,
    decimal Cena);
