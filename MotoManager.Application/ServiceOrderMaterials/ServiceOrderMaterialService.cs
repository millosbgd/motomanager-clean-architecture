using MotoManager.Application.Abstractions;
using MotoManager.Application.ServiceOrderMaterials;
using MotoManager.Domain.Entities;
using System.Linq;

namespace MotoManager.Application.ServiceOrderMaterials;

public class ServiceOrderMaterialService
{
    private readonly IServiceOrderMaterialRepository _repository;

    public ServiceOrderMaterialService(IServiceOrderMaterialRepository repository)
    {
        _repository = repository;
    }

    public async System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<ServiceOrderMaterialDto>> GetAllByServiceOrderIdAsync(int serviceOrderId)
    {
        var materials = await _repository.GetAllByServiceOrderIdAsync(serviceOrderId);
        return materials.Select(m => new ServiceOrderMaterialDto(
            m.Id,
            m.ServiceOrderId,
            m.MaterialId,
            m.Material?.Naziv ?? "",
            m.Kolicina,
            m.JedinicnaCena,
            m.UkupnaCena));
    }

    public async System.Threading.Tasks.Task<ServiceOrderMaterialDto?> GetByIdAsync(int id)
    {
        var material = await _repository.GetByIdAsync(id);
        if (material == null) return null;

        return new ServiceOrderMaterialDto(
            material.Id,
            material.ServiceOrderId,
            material.MaterialId,
            material.Material?.Naziv ?? "",
            material.Kolicina,
            material.JedinicnaCena,
            material.UkupnaCena);
    }

    public async System.Threading.Tasks.Task<ServiceOrderMaterialDto> CreateAsync(CreateServiceOrderMaterialRequest request)
    {
        var material = new ServiceOrderMaterial
        {
            ServiceOrderId = request.ServiceOrderId,
            MaterialId = request.MaterialId,
            Kolicina = request.Kolicina,
            JedinicnaCena = request.JedinicnaCena,
            UkupnaCena = request.UkupnaCena
        };

        var created = await _repository.AddAsync(material);
        return new ServiceOrderMaterialDto(
            created.Id,
            created.ServiceOrderId,
            created.MaterialId,
            created.Material?.Naziv ?? "",
            created.Kolicina,
            created.JedinicnaCena,
            created.UkupnaCena);
    }

    public async System.Threading.Tasks.Task<ServiceOrderMaterialDto> UpdateAsync(UpdateServiceOrderMaterialRequest request)
    {
        var material = new ServiceOrderMaterial
        {
            Id = request.Id,
            ServiceOrderId = request.ServiceOrderId,
            MaterialId = request.MaterialId,
            Kolicina = request.Kolicina,
            JedinicnaCena = request.JedinicnaCena,
            UkupnaCena = request.UkupnaCena
        };

        var updated = await _repository.UpdateAsync(material);
        return new ServiceOrderMaterialDto(
            updated.Id,
            updated.ServiceOrderId,
            updated.MaterialId,
            updated.Material?.Naziv ?? "",
            updated.Kolicina,
            updated.JedinicnaCena,
            updated.UkupnaCena);
    }

    public async System.Threading.Tasks.Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}
