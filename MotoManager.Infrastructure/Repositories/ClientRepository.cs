using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
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
        var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
        var pageSizeParam = new SqlParameter("@PageSize", pageSize);

        var clients = await _context.Clients
            .FromSqlRaw("EXEC sp_GetClientsPaged @PageNumber, @PageSize", pageNumberParam, pageSizeParam)
            .ToListAsync();

        if (clients.Count == 0)
        {
            return (new List<Client>(), 0, pageNumber, pageSize, 0);
        }

        var firstClient = clients[0];
        var totalCount = (int)_context.Entry(firstClient).Property("TotalCount").CurrentValue!;
        var currentPage = (int)_context.Entry(firstClient).Property("CurrentPage").CurrentValue!;
        var totalPages = (int)_context.Entry(firstClient).Property("TotalPages").CurrentValue!;

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
