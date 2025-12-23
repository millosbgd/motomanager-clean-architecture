using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using MotoManager.Application.Abstractions;
using MotoManager.Domain.Entities;
using MotoManager.Infrastructure.Data;

namespace MotoManager.Infrastructure.Repositories;

public class MaterialRepository : IMaterialRepository
{
    private readonly AppDbContext _context;

    public MaterialRepository(AppDbContext context)
    {
        _context = context;
    }

    public async System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Material>> GetAllAsync()
    {
        return await _context.Materials.ToListAsync();
    }

    public async System.Threading.Tasks.Task<(System.Collections.Generic.IEnumerable<Material> Items, int TotalCount, int CurrentPage, int PageSize, int TotalPages)> GetAllPagedAsync(int pageNumber, int pageSize)
    {
        var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
        var pageSizeParam = new SqlParameter("@PageSize", pageSize);

        var materials = await _context.Materials
            .FromSqlRaw("EXEC sp_GetMaterialsPaged @PageNumber, @PageSize", pageNumberParam, pageSizeParam)
            .ToListAsync();

        if (materials.Count == 0)
        {
            return (new System.Collections.Generic.List<Material>(), 0, pageNumber, pageSize, 0);
        }

        var firstMaterial = materials[0];
        var totalCount = (int)_context.Entry(firstMaterial).Property("TotalCount").CurrentValue!;
        var currentPage = (int)_context.Entry(firstMaterial).Property("CurrentPage").CurrentValue!;
        var totalPages = (int)_context.Entry(firstMaterial).Property("TotalPages").CurrentValue!;

        return (materials, totalCount, currentPage, pageSize, totalPages);
    }

    public async System.Threading.Tasks.Task<Material?> GetByIdAsync(int id)
    {
        return await _context.Materials.FindAsync(id);
    }

    public async System.Threading.Tasks.Task<Material> AddAsync(Material material)
    {
        _context.Materials.Add(material);
        await _context.SaveChangesAsync();
        return material;
    }

    public async System.Threading.Tasks.Task<Material> UpdateAsync(Material material)
    {
        _context.Materials.Update(material);
        await _context.SaveChangesAsync();
        return material;
    }

    public async System.Threading.Tasks.Task DeleteAsync(int id)
    {
        var material = await _context.Materials.FindAsync(id);
        if (material != null)
        {
            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();
        }
    }
}
