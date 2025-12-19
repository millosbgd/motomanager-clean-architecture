using MotoManager.Domain.Entities;

namespace MotoManager.Application.Abstractions;

public interface ISektorRepository
{
    Task<IEnumerable<Sektor>> GetAllAsync();
    Task<Sektor?> GetByIdAsync(int id);
    Task<Sektor> CreateAsync(Sektor sektor);
    Task<Sektor?> UpdateAsync(Sektor sektor);
    Task<bool> DeleteAsync(int id);
}
