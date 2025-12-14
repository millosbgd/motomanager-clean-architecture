using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotoManager.Application.Abstractions;
using MotoManager.Domain.Entities;

namespace MotoManager.Application.Clients;

public class ClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<IEnumerable<ClientDto>> GetAllClientsAsync()
    {
        var clients = await _clientRepository.GetAllAsync();
        return clients.Select(c => new ClientDto(c.Id, c.Naziv, c.Adresa, c.Grad, c.PIB, c.Telefon, c.Email));
    }

    public async Task<ClientDto?> GetClientByIdAsync(int id)
    {
        var client = await _clientRepository.GetByIdAsync(id);
        return client == null ? null : new ClientDto(client.Id, client.Naziv, client.Adresa, client.Grad, client.PIB, client.Telefon, client.Email);
    }

    public async Task<ClientDto> CreateClientAsync(CreateClientRequest request)
    {
        var client = new Client
        {
            Naziv = request.Naziv,
            Adresa = request.Adresa,
            Grad = request.Grad,
            PIB = request.PIB,
            Telefon = request.Telefon,
            Email = request.Email
        };

        var created = await _clientRepository.CreateAsync(client);
        return new ClientDto(created.Id, created.Naziv, created.Adresa, created.Grad, created.PIB, created.Telefon, created.Email);
    }

    public async Task<ClientDto?> UpdateClientAsync(UpdateClientRequest request)
    {
        var client = new Client
        {
            Id = request.Id,
            Naziv = request.Naziv,
            Adresa = request.Adresa,
            Grad = request.Grad,
            PIB = request.PIB,
            Telefon = request.Telefon,
            Email = request.Email
        };

        var updated = await _clientRepository.UpdateAsync(client);
        return updated == null ? null : new ClientDto(updated.Id, updated.Naziv, updated.Adresa, updated.Grad, updated.PIB, updated.Telefon, updated.Email);
    }

    public async Task<bool> DeleteClientAsync(int id)
    {
        return await _clientRepository.DeleteAsync(id);
    }
}
