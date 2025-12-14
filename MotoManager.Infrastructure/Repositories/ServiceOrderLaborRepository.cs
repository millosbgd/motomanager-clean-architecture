using Microsoft.EntityFrameworkCore;
using MotoManager.Application.Abstractions;
using MotoManager.Domain.Entities;
using MotoManager.Infrastructure.Data;
using System.Linq;

namespace MotoManager.Infrastructure.Repositories;

public class ServiceOrderLaborRepository : IServiceOrderLaborRepository
{
    private readonly AppDbContext _context;

    public ServiceOrderLaborRepository(AppDbContext context)
    {
        _context = context;
    }

    public async System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<ServiceOrderLabor>> GetAllByServiceOrderIdAsync(int serviceOrderId)
    {
        return await _context.ServiceOrderLabors
            .Where(l => l.ServiceOrderId == serviceOrderId)
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task<ServiceOrderLabor?> GetByIdAsync(int id)
    {
        return await _context.ServiceOrderLabors.FindAsync(id);
    }

    public async System.Threading.Tasks.Task<ServiceOrderLabor> AddAsync(ServiceOrderLabor labor)
    {
        _context.ServiceOrderLabors.Add(labor);
        await _context.SaveChangesAsync();
        return labor;
    }

    public async System.Threading.Tasks.Task<ServiceOrderLabor> UpdateAsync(ServiceOrderLabor labor)
    {
        _context.ServiceOrderLabors.Update(labor);
        await _context.SaveChangesAsync();
        return labor;
    }

    public async System.Threading.Tasks.Task DeleteAsync(int id)
    {
        var labor = await _context.ServiceOrderLabors.FindAsync(id);
        if (labor != null)
        {
            _context.ServiceOrderLabors.Remove(labor);
            await _context.SaveChangesAsync();
        }
    }
}
