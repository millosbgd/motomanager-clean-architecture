using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MotoManager.Domain.Entities;

namespace MotoManager.Application.Abstractions;

public interface IVehicleRepository
{
    Task<List<Vehicle>> GetAllAsync();
    Task<(IEnumerable<Vehicle> Items, int TotalCount, int CurrentPage, int PageSize, int TotalPages)> GetAllPagedAsync(int pageNumber, int pageSize);
    Task<Vehicle?> GetByIdAsync(int id);
    Task<Vehicle> AddAsync(Vehicle vehicle);
    Task UpdateAsync(Vehicle vehicle);
    Task DeleteAsync(int id);
}
