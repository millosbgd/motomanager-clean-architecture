using MotoManager.Application.Abstractions;
using MotoManager.Domain.Entities;

namespace MotoManager.Application.PurchaseInvoices;

public class PurchaseInvoiceService
{
    private readonly IPurchaseInvoiceRepository _purchaseInvoiceRepository;

    public PurchaseInvoiceService(IPurchaseInvoiceRepository purchaseInvoiceRepository)
    {
        _purchaseInvoiceRepository = purchaseInvoiceRepository;
    }

    public async Task<IEnumerable<PurchaseInvoiceDto>> GetAllPurchaseInvoicesAsync()
    {
        var invoices = await _purchaseInvoiceRepository.GetAllAsync();
        return invoices.Select(i => new PurchaseInvoiceDto(
            i.Id, 
            i.BrojRacuna, 
            i.Datum, 
            i.DobavljacId, 
            i.Dobavljac.Naziv,
            i.VoziloId,
            i.Vozilo?.Model,
            i.Vozilo?.Plate,
            i.IznosNeto, 
            i.IznosPDV, 
            i.IznosBruto));
    }

    public async Task<PurchaseInvoiceDto?> GetPurchaseInvoiceByIdAsync(int id)
    {
        var invoice = await _purchaseInvoiceRepository.GetByIdAsync(id);
        return invoice == null ? null : new PurchaseInvoiceDto(
            invoice.Id, 
            invoice.BrojRacuna, 
            invoice.Datum, 
            invoice.DobavljacId, 
            invoice.Dobavljac.Naziv,
            invoice.VoziloId,
            invoice.Vozilo?.Model,
            invoice.Vozilo?.Plate,
            invoice.IznosNeto, 
            invoice.IznosPDV, 
            invoice.IznosBruto);
    }

    public async Task<PurchaseInvoiceDto> CreatePurchaseInvoiceAsync(CreatePurchaseInvoiceRequest request)
    {
        var invoice = new PurchaseInvoice
        {
            BrojRacuna = request.BrojRacuna,
            Datum = request.Datum,
            DobavljacId = request.DobavljacId,
            VoziloId = request.VoziloId,
            IznosNeto = request.IznosNeto,
            IznosPDV = request.IznosPDV,
            IznosBruto = request.IznosBruto
        };

        var created = await _purchaseInvoiceRepository.CreateAsync(invoice);
        return new PurchaseInvoiceDto(
            created.Id, 
            created.BrojRacuna, 
            created.Datum, 
            created.DobavljacId, 
            created.Dobavljac.Naziv,
            created.VoziloId,
            created.Vozilo?.Model,
            created.Vozilo?.Plate,
            created.IznosNeto, 
            created.IznosPDV, 
            created.IznosBruto);
    }

    public async Task<PurchaseInvoiceDto?> UpdatePurchaseInvoiceAsync(UpdatePurchaseInvoiceRequest request)
    {
        var invoice = new PurchaseInvoice
        {
            Id = request.Id,
            BrojRacuna = request.BrojRacuna,
            Datum = request.Datum,
            DobavljacId = request.DobavljacId,
            VoziloId = request.VoziloId,
            IznosNeto = request.IznosNeto,
            IznosPDV = request.IznosPDV,
            IznosBruto = request.IznosBruto
        };

        var updated = await _purchaseInvoiceRepository.UpdateAsync(invoice);
        return updated == null ? null : new PurchaseInvoiceDto(
            updated.Id, 
            updated.BrojRacuna, 
            updated.Datum, 
            updated.DobavljacId, 
            updated.Dobavljac.Naziv,
            updated.VoziloId,
            updated.Vozilo?.Model,
            updated.Vozilo?.Plate,
            updated.IznosNeto, 
            updated.IznosPDV, 
            updated.IznosBruto);
    }

    public async Task<bool> DeletePurchaseInvoiceAsync(int id)
    {
        return await _purchaseInvoiceRepository.DeleteAsync(id);
    }
}
