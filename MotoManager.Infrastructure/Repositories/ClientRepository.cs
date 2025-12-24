using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dapper;
using MotoManager.Application.Abstractions;
using MotoManager.Domain.Entities;
using MotoManager.Infrastructure.Data;

namespace MotoManager.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;

    public ClientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task<(IEnumerable<Client> Items, int TotalCount, int CurrentPage, int PageSize, int TotalPages)> GetAllPagedAsync(int pageNumber, int pageSize)
    {
        var connection = _context.Database.GetDbConnection();
        
        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync();
        }
        
        var results = await connection.QueryAsync<dynamic>(
            "sp_GetClientsPaged",
            new { PageNumber = pageNumber, PageSize = pageSize },
            commandType: System.Data.CommandType.StoredProcedure
        );

        var resultsList = results.ToList();
        
        if (resultsList.Count == 0)
        {
            return (new List<Client>(), 0, pageNumber, pageSize, 0);
        }

        var first = resultsList[0];
        int totalCount = (int)first.TotalCount;
        int currentPage = (int)first.CurrentPage;
        int totalPages = (int)first.TotalPages;

        var clients = resultsList.Select(r => new Client
        {
            Id = (int)r.Id,
            Naziv = (string)r.Naziv ?? string.Empty,
            Adresa = (string)r.Adresa ?? string.Empty,
            Grad = string.Empty,
            PIB = (string)r.PIB ?? string.Empty,
            Telefon = (string)r.Telefon ?? string.Empty,
            Email = (string)r.Email ?? string.Empty
        }).ToList();

        return (clients, totalCount, currentPage, pageSize, totalPages);
    }

    public async Task<Client?> GetByIdAsync(int id)
    {
        return await _context.Clients.FindAsync(id);
    }

    public async Task<Client> CreateAsync(Client client)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return client;
    }

    public async Task<Client?> UpdateAsync(Client client)
    {
        var existing = await _context.Clients.FindAsync(client.Id);
        if (existing == null) return null;

        existing.Naziv = client.Naziv;
        existing.Adresa = client.Adresa;
        existing.Grad = client.Grad;
        existing.PIB = client.PIB;
        existing.Telefon = client.Telefon;
        existing.Email = client.Email;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null) return false;

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return true;
    }
}
