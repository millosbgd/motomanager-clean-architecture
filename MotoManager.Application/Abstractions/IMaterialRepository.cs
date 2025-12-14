namespace MotoManager.Application.Abstractions;

public interface IMaterialRepository
{
    System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Domain.Entities.Material>> GetAllAsync();
    System.Threading.Tasks.Task<Domain.Entities.Material?> GetByIdAsync(int id);
    System.Threading.Tasks.Task<Domain.Entities.Material> AddAsync(Domain.Entities.Material material);
    System.Threading.Tasks.Task<Domain.Entities.Material> UpdateAsync(Domain.Entities.Material material);
    System.Threading.Tasks.Task DeleteAsync(int id);
}
