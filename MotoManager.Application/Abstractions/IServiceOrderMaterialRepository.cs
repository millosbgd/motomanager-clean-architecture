namespace MotoManager.Application.Abstractions;

public interface IServiceOrderMaterialRepository
{
    System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Domain.Entities.ServiceOrderMaterial>> GetAllByServiceOrderIdAsync(int serviceOrderId);
    System.Threading.Tasks.Task<Domain.Entities.ServiceOrderMaterial?> GetByIdAsync(int id);
    System.Threading.Tasks.Task<Domain.Entities.ServiceOrderMaterial> AddAsync(Domain.Entities.ServiceOrderMaterial material);
    System.Threading.Tasks.Task<Domain.Entities.ServiceOrderMaterial> UpdateAsync(Domain.Entities.ServiceOrderMaterial material);
    System.Threading.Tasks.Task DeleteAsync(int id);
}
