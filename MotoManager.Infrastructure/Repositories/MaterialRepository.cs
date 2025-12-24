using System.Linq;
using Microsoft.EntityFrameworkCore;
using Dapper;
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
        var connection = _context.Database.GetDbConnection();
        
        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync();
        }
        
        var results = await connection.QueryAsync<dynamic>(
            "sp_GetMaterialsPaged",
            new { PageNumber = pageNumber, PageSize = pageSize },
            commandType: System.Data.CommandType.StoredProcedure
        );

        var resultsList = results.ToList();
        
        if (resultsList.Count == 0)
        {
            return (new System.Collections.Generic.List<Material>(), 0, pageNumber, pageSize, 0);
        }

        var first = resultsList[0];
        int totalCount = (int)first.TotalCount;
        int currentPage = (int)first.CurrentPage;
        int totalPages = (int)first.TotalPages;

        var materials = resultsList.Select(r => new Material
        {
            Id = (int)r.Id,
            Naziv = (string)r.Naziv ?? string.Empty,
            JedinicnaCena = (decimal)r.JedinicnaCena
        }).ToList();

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
