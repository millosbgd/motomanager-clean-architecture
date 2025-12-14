using MotoManager.Application.Abstractions;
using MotoManager.Application.ServiceOrderLabors;
using MotoManager.Domain.Entities;
using System.Linq;

namespace MotoManager.Application.ServiceOrderLabors;

public class ServiceOrderLaborService
{
    private readonly IServiceOrderLaborRepository _repository;

    public ServiceOrderLaborService(IServiceOrderLaborRepository repository)
    {
        _repository = repository;
    }

    public async System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<ServiceOrderLaborDto>> GetAllByServiceOrderIdAsync(int serviceOrderId)
    {
        var labors = await _repository.GetAllByServiceOrderIdAsync(serviceOrderId);
        return labors.Select(l => new ServiceOrderLaborDto(
            l.Id,
            l.ServiceOrderId,
            l.OpisRadova,
            l.UkupnoVreme,
            l.Cena));
    }

    public async System.Threading.Tasks.Task<ServiceOrderLaborDto?> GetByIdAsync(int id)
    {
        var labor = await _repository.GetByIdAsync(id);
        if (labor == null) return null;

        return new ServiceOrderLaborDto(
            labor.Id,
            labor.ServiceOrderId,
            labor.OpisRadova,
            labor.UkupnoVreme,
            labor.Cena);
    }

    public async System.Threading.Tasks.Task<ServiceOrderLaborDto> CreateAsync(CreateServiceOrderLaborRequest request)
    {
        var labor = new ServiceOrderLabor
        {
            ServiceOrderId = request.ServiceOrderId,
            OpisRadova = request.OpisRadova,
            UkupnoVreme = request.UkupnoVreme,
            Cena = request.Cena
        };

        var created = await _repository.AddAsync(labor);
        return new ServiceOrderLaborDto(
            created.Id,
            created.ServiceOrderId,
            created.OpisRadova,
            created.UkupnoVreme,
            created.Cena);
    }

    public async System.Threading.Tasks.Task<ServiceOrderLaborDto> UpdateAsync(UpdateServiceOrderLaborRequest request)
    {
        var labor = new ServiceOrderLabor
        {
            Id = request.Id,
            ServiceOrderId = request.ServiceOrderId,
            OpisRadova = request.OpisRadova,
            UkupnoVreme = request.UkupnoVreme,
            Cena = request.Cena
        };

        var updated = await _repository.UpdateAsync(labor);
        return new ServiceOrderLaborDto(
            updated.Id,
            updated.ServiceOrderId,
            updated.OpisRadova,
            updated.UkupnoVreme,
            updated.Cena);
    }

    public async System.Threading.Tasks.Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}
