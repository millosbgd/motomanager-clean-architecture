using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotoManager.Application.Abstractions;
using MotoManager.Domain.Entities;

namespace MotoManager.Application.ServiceOrders;

public class ServiceOrderService
{
    private readonly IServiceOrderRepository _serviceOrderRepository;

    public ServiceOrderService(IServiceOrderRepository serviceOrderRepository)
    {
        _serviceOrderRepository = serviceOrderRepository;
    }

    public async Task<IEnumerable<ServiceOrderDto>> GetAllServiceOrdersAsync()
    {
        var orders = await _serviceOrderRepository.GetAllAsync();
        return orders.Select(o => new ServiceOrderDto(
            o.Id, 
            o.BrojNaloga, 
            o.Datum, 
            o.ClientId, 
            o.Client?.Naziv ?? "",
            o.VehicleId, 
            o.Vehicle?.Model ?? "",
            o.Vehicle?.Plate ?? "",
            o.OpisRada, 
            o.Kilometraza,
            o.KorisnikId,
            o.Korisnik?.ImePrezime ?? ""
        ));
    }

    public async Task<object> GetAllServiceOrdersPagedAsync(int pageNumber, int pageSize)
    {
        var (items, totalCount, currentPage, pageSizeResult, totalPages) = await _serviceOrderRepository.GetAllPagedAsync(pageNumber, pageSize);
        var dtos = items.Select(o => new ServiceOrderDto(
            o.Id, 
            o.BrojNaloga, 
            o.Datum, 
            o.ClientId, 
            o.Client?.Naziv ?? "",
            o.VehicleId, 
            o.Vehicle?.Model ?? "",
            o.Vehicle?.Plate ?? "",
            o.OpisRada, 
            o.Kilometraza,
            o.KorisnikId,
            o.Korisnik?.ImePrezime ?? ""
        ));
        
        return new
        {
            Items = dtos,
            TotalCount = totalCount,
            CurrentPage = currentPage,
            PageSize = pageSizeResult,
            TotalPages = totalPages
        };
    }

    public async Task<ServiceOrderDto?> GetServiceOrderByIdAsync(int id)
    {
        var order = await _serviceOrderRepository.GetByIdAsync(id);
        if (order == null) return null;
        
        return new ServiceOrderDto(
            order.Id, 
            order.BrojNaloga, 
            order.Datum, 
            order.ClientId, 
            order.Client?.Naziv ?? "",
            order.VehicleId, 
            order.Vehicle?.Model ?? "",
            order.Vehicle?.Plate ?? "",
            order.OpisRada, 
            order.Kilometraza,
            order.KorisnikId,
            order.Korisnik?.ImePrezime);
    }

    public async Task<ServiceOrderDto> CreateServiceOrderAsync(CreateServiceOrderRequest request)
    {
        var order = new ServiceOrder
        {
            BrojNaloga = request.BrojNaloga,
            Datum = request.Datum,
            ClientId = request.ClientId,
            VehicleId = request.VehicleId,
            OpisRada = request.OpisRada,
            Kilometraza = request.Kilometraza,
            KorisnikId = request.KorisnikId
        };

        var created = await _serviceOrderRepository.CreateAsync(order);
        return new ServiceOrderDto(
            created.Id, 
            created.BrojNaloga, 
            created.Datum, 
            created.ClientId, 
            created.Client?.Naziv ?? "",
            created.VehicleId, 
            created.Vehicle?.Model ?? "",
            created.Vehicle?.Plate ?? "",
            created.OpisRada, 
            created.Kilometraza,
            created.KorisnikId,
            created.Korisnik?.ImePrezime);
    }

    public async Task<ServiceOrderDto?> UpdateServiceOrderAsync(UpdateServiceOrderRequest request)
    {
        var order = new ServiceOrder
        {
            Id = request.Id,
            BrojNaloga = request.BrojNaloga,
            Datum = request.Datum,
            ClientId = request.ClientId,
            VehicleId = request.VehicleId,
            OpisRada = request.OpisRada,
            Kilometraza = request.Kilometraza,
            KorisnikId = request.KorisnikId
        };

        var updated = await _serviceOrderRepository.UpdateAsync(order);
        if (updated == null) return null;

        return new ServiceOrderDto(
            updated.Id, 
            updated.BrojNaloga, 
            updated.Datum, 
            updated.ClientId, 
            updated.Client?.Naziv ?? "",
            updated.VehicleId, 
            updated.Vehicle?.Model ?? "",
            updated.Vehicle?.Plate ?? "",
            updated.OpisRada, 
            updated.Kilometraza,
            updated.KorisnikId,
            updated.Korisnik?.ImePrezime);
    }

    public async Task<bool> DeleteServiceOrderAsync(int id)
    {
        return await _serviceOrderRepository.DeleteAsync(id);
    }
}
