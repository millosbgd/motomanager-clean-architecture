using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Dapper;
using MotoManager.Application.Abstractions;
using MotoManager.Domain.Entities;
using MotoManager.Infrastructure.Data;

namespace MotoManager.Infrastructure.Repositories;

// Helper class for mapping stored procedure results
internal class VehiclePagedResult
{
    public int Id { get; set; }
    public string Plate { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int ClientId { get; set; }
    public string ClientNaziv { get; set; } = string.Empty;
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}

public class VehicleRepository : IVehicleRepository
{
    private readonly AppDbContext _db;

    public VehicleRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Vehicle>> GetAllAsync()
    {
        return await _db.Vehicles
            .Include(v => v.Client)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<(IEnumerable<Vehicle> Items, int TotalCount, int CurrentPage, int PageSize, int TotalPages)> GetAllPagedAsync(int pageNumber, int pageSize)
    {
        var connection = _db.Database.GetDbConnection();
        
        // Ensure connection is open
        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync();
        }
        
        // Query stored procedure - ONE database call
        var results = await connection.QueryAsync<VehiclePagedResult>(
            "sp_GetVehiclesPaged",
            new { PageNumber = pageNumber, PageSize = pageSize },
            commandType: System.Data.CommandType.StoredProcedure
        );

        var resultsList = results.ToList();
        
        // DEBUG: Log first result to see what Dapper is mapping
        if (resultsList.Count > 0)
        {
            var first = resultsList[0];
            Console.WriteLine($"DEBUG: First vehicle - Id={first.Id}, Plate={first.Plate}, Model={first.Model}, ClientId={first.ClientId}, ClientNaziv='{first.ClientNaziv}'");
        }
        
        if (resultsList.Count == 0)
        {
            return (new List<Vehicle>(), 0, pageNumber, pageSize, 0);
        }

        var metadata = resultsList[0];
        
        // Map results directly from stored procedure - NO additional query!
        var vehicles = resultsList.Select(r => new Vehicle
        {
            Id = r.Id,
            Model = r.Model,
            Plate = r.Plate,
            ClientId = r.ClientId,
            Client = new Client 
            { 
                Id = r.ClientId, 
                Naziv = r.ClientNaziv 
            }
        }).ToList();

        return (vehicles, metadata.TotalCount, metadata.CurrentPage, metadata.PageSize, metadata.TotalPages);
    }

    public async Task<Vehicle?> GetByIdAsync(int id)
    {
        return await _db.Vehicles
            .Include(v => v.Client)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Vehicle> AddAsync(Vehicle vehicle)
    {
        _db.Vehicles.Add(vehicle);
        await _db.SaveChangesAsync();
        return vehicle;
    }

    public async Task UpdateAsync(Vehicle vehicle)
    {
        _db.Vehicles.Update(vehicle);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _db.Vehicles.FindAsync(id);
        if (existing is null) return;
        _db.Vehicles.Remove(existing);
        await _db.SaveChangesAsync();
    }
}
