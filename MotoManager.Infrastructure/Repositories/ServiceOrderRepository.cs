using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
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
        var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
        var pageSizeParam = new SqlParameter("@PageSize", pageSize);

        var serviceOrders = await _context.ServiceOrders
            .FromSqlRaw("EXEC sp_GetServiceOrdersPaged @PageNumber, @PageSize", pageNumberParam, pageSizeParam)
            .AsNoTracking()
            .ToListAsync();

        if (serviceOrders.Count == 0)
        {
            return (new List<ServiceOrder>(), 0, pageNumber, pageSize, 0);
        }

        var firstOrder = serviceOrders[0];
        var totalCount = (int)_context.Entry(firstOrder).Property("TotalCount").CurrentValue!;
        var currentPage = (int)_context.Entry(firstOrder).Property("CurrentPage").CurrentValue!;
        var totalPages = (int)_context.Entry(firstOrder).Property("TotalPages").CurrentValue!;

        // Load navigation properties
        var orderIds = serviceOrders.Select(so => so.Id).ToList();
        var ordersWithNav = await _context.ServiceOrders
            .Include(so => so.Client)
            .Include(so => so.Vehicle)
            .Include(so => so.Korisnik)
            .Where(so => orderIds.Contains(so.Id))
            .AsNoTracking()
            .ToListAsync();

        return (ordersWithNav, totalCount, currentPage, pageSize, totalPages);
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
