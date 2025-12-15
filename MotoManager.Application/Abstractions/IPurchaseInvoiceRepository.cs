using MotoManager.Domain.Entities;

namespace MotoManager.Application.Abstractions;

public interface IPurchaseInvoiceRepository
{
    Task<IEnumerable<PurchaseInvoice>> GetAllAsync(DateTime? datumOd = null, DateTime? datumDo = null, int? dobavljacId = null, int? voziloId = null);
    Task<PurchaseInvoice?> GetByIdAsync(int id);
    Task<PurchaseInvoice> CreateAsync(PurchaseInvoice purchaseInvoice);
    Task<PurchaseInvoice?> UpdateAsync(PurchaseInvoice purchaseInvoice);
    Task<bool> DeleteAsync(int id);
}
