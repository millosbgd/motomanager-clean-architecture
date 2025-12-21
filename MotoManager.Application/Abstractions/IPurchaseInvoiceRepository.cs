using MotoManager.Domain.Entities;
using MotoManager.Application.PurchaseInvoices;

namespace MotoManager.Application.Abstractions;

public interface IPurchaseInvoiceRepository
{
    Task<PagedResult<PurchaseInvoice>> GetAllPagedAsync(
        DateTime? datumOd = null, 
        DateTime? datumDo = null, 
        int? dobavljacId = null, 
        int? voziloId = null,
        int pageNumber = 1,
        int pageSize = 20);
    Task<PurchaseInvoice?> GetByIdAsync(int id);
    Task<PurchaseInvoice> CreateAsync(PurchaseInvoice purchaseInvoice);
    Task<PurchaseInvoice?> UpdateAsync(PurchaseInvoice purchaseInvoice);
    Task<bool> DeleteAsync(int id);
}
