using System.Collections.Generic;
using System.Threading.Tasks;
using MotoManager.Domain.Entities;

namespace MotoManager.Application.Abstractions;

public interface IServiceOrderRepository
{
    Task<IEnumerable<ServiceOrder>> GetAllAsync();
    Task<(IEnumerable<ServiceOrder> Items, int TotalCount, int CurrentPage, int PageSize, int TotalPages)> GetAllPagedAsync(int pageNumber, int pageSize);
    Task<ServiceOrder?> GetByIdAsync(int id);
    Task<ServiceOrder> CreateAsync(ServiceOrder serviceOrder);
    Task<ServiceOrder?> UpdateAsync(ServiceOrder serviceOrder);
    Task<bool> DeleteAsync(int id);
}
