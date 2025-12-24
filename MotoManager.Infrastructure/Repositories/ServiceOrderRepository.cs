using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dapper;
using MotoManager.Application.Abstractions;
using MotoManager.Domain.Entities;
using MotoManager.Infrastructure.Data;

namespace MotoManager.Infrastructure.Repositories;

public class ServiceOrderRepository : IServiceOrderRepository
{
    private readonly AppDbContext _context;

    public ServiceOrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ServiceOrder>> GetAllAsync()
    {
        return await _context.ServiceOrders
            .Include(so => so.Client)
            .Include(so => so.Vehicle)
            .Include(so => so.Korisnik)
            .ToListAsync();
    }

    public async Task<(IEnumerable<ServiceOrder> Items, int TotalCount, int CurrentPage, int PageSize, int TotalPages)> GetAllPagedAsync(int pageNumber, int pageSize)
    {
        var connection = _context.Database.GetDbConnection();
        
        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync();
        }
        
        var results = await connection.QueryAsync<dynamic>(
            "sp_GetServiceOrdersPaged",
            new { PageNumber = pageNumber, PageSize = pageSize },
            commandType: System.Data.CommandType.StoredProcedure
        );

        var resultsList = results.ToList();
        
        if (resultsList.Count == 0)
        {
            return (new List<ServiceOrder>(), 0, pageNumber, pageSize, 0);
        }

        var first = resultsList[0];
        int totalCount = (int)first.TotalCount;
        int currentPage = (int)first.CurrentPage;
        int totalPages = (int)first.TotalPages;

        var serviceOrders = resultsList.Select(r => new ServiceOrder
        {
            Id = (int)r.Id,
            BrojNaloga = (string)r.BrojNaloga ?? string.Empty,
            Datum = (System.DateTime)r.Datum,
            ClientId = (int)r.ClientId,
            VehicleId = (int)r.VehicleId,
            OpisRada = (string)r.OpisRada ?? string.Empty,
            Kilometraza = (int)r.Kilometraza,
            KorisnikId = r.KorisnikId != null ? (string)r.KorisnikId : null,
            // Navigation properties for DTO mapping
            Client = new Client 
            { 
                Id = (int)r.ClientId,
                Naziv = (string)r.ClientNaziv ?? string.Empty
            },
            Vehicle = new Vehicle 
            { 
                Id = (int)r.VehicleId,
                Model = (string)r.VehicleModel ?? string.Empty,
                Plate = (string)r.VehiclePlate ?? string.Empty
            },
            Korisnik = r.KorisnikId != null && r.KorisnikImePrezime != null 
                ? new Korisnik 
                { 
                    Id = (string)r.KorisnikId,
                    ImePrezime = (string)r.KorisnikImePrezime 
                }
                : null
        }).ToList();

        return (serviceOrders, totalCount, currentPage, pageSize, totalPages);
    }

    public async Task<ServiceOrder?> GetByIdAsync(int id)
    {
        return await _context.ServiceOrders
            .Include(so => so.Client)
            .Include(so => so.Vehicle)
            .Include(so => so.Korisnik)
            .FirstOrDefaultAsync(so => so.Id == id);
    }

    public async Task<ServiceOrder> CreateAsync(ServiceOrder serviceOrder)
    {
        _context.ServiceOrders.Add(serviceOrder);
        await _context.SaveChangesAsync();
        
        // Reload with navigation properties
        return (await GetByIdAsync(serviceOrder.Id))!;
    }

    public async Task<ServiceOrder?> UpdateAsync(ServiceOrder serviceOrder)
    {
        var existing = await _context.ServiceOrders.FindAsync(serviceOrder.Id);
        if (existing == null) return null;

        existing.BrojNaloga = serviceOrder.BrojNaloga;
        existing.Datum = serviceOrder.Datum;
        existing.ClientId = serviceOrder.ClientId;
        existing.VehicleId = serviceOrder.VehicleId;
        existing.OpisRada = serviceOrder.OpisRada;
        existing.Kilometraza = serviceOrder.Kilometraza;
        existing.KorisnikId = serviceOrder.KorisnikId;

        await _context.SaveChangesAsync();
        
        // Reload with navigation properties
        return await GetByIdAsync(existing.Id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var order = await _context.ServiceOrders.FindAsync(id);
        if (order == null) return false;

        _context.ServiceOrders.Remove(order);
        await _context.SaveChangesAsync();
        return true;
    }
}
