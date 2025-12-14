using Microsoft.EntityFrameworkCore;
using MotoManager.Application.Abstractions;
using MotoManager.Domain.Entities;
using MotoManager.Infrastructure.Data;
using System.Linq;

namespace MotoManager.Infrastructure.Repositories;

public class ServiceOrderMaterialRepository : IServiceOrderMaterialRepository
{
    private readonly AppDbContext _context;

    public ServiceOrderMaterialRepository(AppDbContext context)
    {
        _context = context;
    }

    public async System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<ServiceOrderMaterial>> GetAllByServiceOrderIdAsync(int serviceOrderId)
    {
        return await _context.ServiceOrderMaterials
            .Include(m => m.Material)
            .Where(m => m.ServiceOrderId == serviceOrderId)
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task<ServiceOrderMaterial?> GetByIdAsync(int id)
    {
        return await _context.ServiceOrderMaterials
            .Include(m => m.Material)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async System.Threading.Tasks.Task<ServiceOrderMaterial> AddAsync(ServiceOrderMaterial material)
    {
        _context.ServiceOrderMaterials.Add(material);
        await _context.SaveChangesAsync();
        return material;
    }

    public async System.Threading.Tasks.Task<ServiceOrderMaterial> UpdateAsync(ServiceOrderMaterial material)
    {
        _context.ServiceOrderMaterials.Update(material);
        await _context.SaveChangesAsync();
        return material;
    }

    public async System.Threading.Tasks.Task DeleteAsync(int id)
    {
        var material = await _context.ServiceOrderMaterials.FindAsync(id);
        if (material != null)
        {
            _context.ServiceOrderMaterials.Remove(material);
            await _context.SaveChangesAsync();
        }
    }
}
