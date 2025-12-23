using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using MotoManager.Application.Abstractions;
using MotoManager.Domain.Entities;
using MotoManager.Infrastructure.Data;

namespace MotoManager.Infrastructure.Repositories;

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
        var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
        var pageSizeParam = new SqlParameter("@PageSize", pageSize);

        var vehicles = await _db.Vehicles
            .FromSqlRaw("EXEC sp_GetVehiclesPaged @PageNumber, @PageSize", pageNumberParam, pageSizeParam)
            .AsNoTracking()
            .ToListAsync();

        if (vehicles.Count == 0)
        {
            return (new List<Vehicle>(), 0, pageNumber, pageSize, 0);
        }

        var firstVehicle = vehicles[0];
        var totalCount = (int)_db.Entry(firstVehicle).Property("TotalCount").CurrentValue!;
        var currentPage = (int)_db.Entry(firstVehicle).Property("CurrentPage").CurrentValue!;
        var totalPages = (int)_db.Entry(firstVehicle).Property("TotalPages").CurrentValue!;

        // Load Client navigation properties
        var vehicleIds = vehicles.Select(v => v.Id).ToList();
        var vehiclesWithClients = await _db.Vehicles
            .Include(v => v.Client)
            .Where(v => vehicleIds.Contains(v.Id))
            .AsNoTracking()
            .ToListAsync();

        return (vehiclesWithClients, totalCount, currentPage, pageSize, totalPages);
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
