using System.Collections.Generic;
using System.Threading.Tasks;
using MotoManager.Domain.Entities;

namespace MotoManager.Application.Abstractions;

public interface IServiceOrderRepository
{
    Task<IEnumerable<ServiceOrder>> GetAllAsync();
    Task<ServiceOrder?> GetByIdAsync(int id);
    Task<ServiceOrder> CreateAsync(ServiceOrder serviceOrder);
    Task<ServiceOrder?> UpdateAsync(ServiceOrder serviceOrder);
    Task<bool> DeleteAsync(int id);
}
