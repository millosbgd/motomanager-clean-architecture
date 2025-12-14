namespace MotoManager.Application.Clients;

public record ClientDto(int Id, string Naziv, string Adresa, string Grad, string? PIB, string Telefon, string Email);

public record CreateClientRequest(string Naziv, string Adresa, string Grad, string? PIB, string Telefon, string Email);

public record UpdateClientRequest(int Id, string Naziv, string Adresa, string Grad, string? PIB, string Telefon, string Email);
