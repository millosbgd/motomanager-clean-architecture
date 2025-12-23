using System.Collections.Generic;
using System.Threading.Tasks;
using MotoManager.Domain.Entities;

namespace MotoManager.Application.Abstractions;

public interface IClientRepository
{
    Task<IEnumerable<Client>> GetAllAsync();
    Task<(IEnumerable<Client> Items, int TotalCount, int CurrentPage, int PageSize, int TotalPages)> GetAllPagedAsync(int pageNumber, int pageSize);
    Task<Client?> GetByIdAsync(int id);
    Task<Client> CreateAsync(Client client);
    Task<Client?> UpdateAsync(Client client);
    Task<bool> DeleteAsync(int id);
}
