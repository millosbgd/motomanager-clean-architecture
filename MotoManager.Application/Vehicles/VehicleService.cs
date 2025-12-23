using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotoManager.Application.Abstractions;
using MotoManager.Domain.Entities;

namespace MotoManager.Application.Vehicles;

public class VehicleService
{
    private readonly IVehicleRepository _repo;

    public VehicleService(IVehicleRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<VehicleDto>> GetAllAsync()
    {
        var vehicles = await _repo.GetAllAsync();
        return vehicles.Select(v => new VehicleDto(
            v.Id,
            v.Model,
            v.Plate,
            v.ClientId,
            v.Client?.Naziv ?? ""
        )).ToList();
    }

    public async Task<object> GetAllPagedAsync(int pageNumber, int pageSize)
    {
        var (items, totalCount, currentPage, pageSizeResult, totalPages) = await _repo.GetAllPagedAsync(pageNumber, pageSize);
        var dtos = items.Select(v => new VehicleDto(
            v.Id,
            v.Model,
            v.Plate,
            v.ClientId,
            ""  // ClientNaziv will be in the result from procedure
        )).ToList();
        
        return new
        {
            Items = dtos,
            TotalCount = totalCount,
            CurrentPage = currentPage,
            PageSize = pageSizeResult,
            TotalPages = totalPages
        };
    }

    public async Task<VehicleDto?> GetByIdAsync(int id)
    {
        var v = await _repo.GetByIdAsync(id);
        if (v is null) return null;

        return new VehicleDto(
            v.Id,
            v.Model,
            v.Plate,
            v.ClientId,
            v.Client?.Naziv ?? ""
        );
    }

    public async Task<VehicleDto> CreateAsync(CreateVehicleRequest request)
    {
        var entity = new Vehicle
        {
            Model = request.Model,
            Plate = request.Plate,
            ClientId = request.ClientId
        };

        var created = await _repo.AddAsync(entity);

        return new VehicleDto(
            created.Id,
            created.Model,
            created.Plate,
            created.ClientId,
            created.Client?.Naziv ?? ""
        );
    }

    public async Task<bool> UpdateAsync(UpdateVehicleRequest request)
    {
        var existing = await _repo.GetByIdAsync(request.Id);
        if (existing is null) return false;

        existing.Model = request.Model;
        existing.Plate = request.Plate;
        existing.ClientId = request.ClientId;
        await _repo.UpdateAsync(existing);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing is null) return false;

        await _repo.DeleteAsync(id);
        return true;
    }
}
