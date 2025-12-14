namespace MotoManager.Application.Abstractions;

public interface IServiceOrderLaborRepository
{
    System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Domain.Entities.ServiceOrderLabor>> GetAllByServiceOrderIdAsync(int serviceOrderId);
    System.Threading.Tasks.Task<Domain.Entities.ServiceOrderLabor?> GetByIdAsync(int id);
    System.Threading.Tasks.Task<Domain.Entities.ServiceOrderLabor> AddAsync(Domain.Entities.ServiceOrderLabor labor);
    System.Threading.Tasks.Task<Domain.Entities.ServiceOrderLabor> UpdateAsync(Domain.Entities.ServiceOrderLabor labor);
    System.Threading.Tasks.Task DeleteAsync(int id);
}
