using MotoManager.Application.Abstractions;
using MotoManager.Application.Materials;
using MotoManager.Domain.Entities;
using System.Linq;

namespace MotoManager.Application.Materials;

public class MaterialService
{
    private readonly IMaterialRepository _repository;

    public MaterialService(IMaterialRepository repository)
    {
        _repository = repository;
    }

    public async System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<MaterialDto>> GetAllAsync()
    {
        var materials = await _repository.GetAllAsync();
        return materials.Select(m => new MaterialDto(m.Id, m.Naziv, m.JedinicnaCena));
    }

    public async System.Threading.Tasks.Task<object> GetAllPagedAsync(int pageNumber, int pageSize)
    {
        var (items, totalCount, currentPage, pageSizeResult, totalPages) = await _repository.GetAllPagedAsync(pageNumber, pageSize);
        var dtos = items.Select(m => new MaterialDto(m.Id, m.Naziv, m.JedinicnaCena));
        
        return new
        {
            Items = dtos,
            TotalCount = totalCount,
            CurrentPage = currentPage,
            PageSize = pageSizeResult,
            TotalPages = totalPages
        };
    }

    public async System.Threading.Tasks.Task<MaterialDto?> GetByIdAsync(int id)
    {
        var material = await _repository.GetByIdAsync(id);
        if (material == null) return null;

        return new MaterialDto(material.Id, material.Naziv, material.JedinicnaCena);
    }

    public async System.Threading.Tasks.Task<MaterialDto> CreateAsync(CreateMaterialRequest request)
    {
        var material = new Material
        {
            Naziv = request.Naziv,
            JedinicnaCena = request.JedinicnaCena
        };

        var created = await _repository.AddAsync(material);
        return new MaterialDto(created.Id, created.Naziv, created.JedinicnaCena);
    }

    public async System.Threading.Tasks.Task<MaterialDto> UpdateAsync(UpdateMaterialRequest request)
    {
        var material = new Material
        {
            Id = request.Id,
            Naziv = request.Naziv,
            JedinicnaCena = request.JedinicnaCena
        };

        var updated = await _repository.UpdateAsync(material);
        return new MaterialDto(updated.Id, updated.Naziv, updated.JedinicnaCena);
    }

    public async System.Threading.Tasks.Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}
