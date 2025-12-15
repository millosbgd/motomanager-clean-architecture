using Microsoft.EntityFrameworkCore;
using MotoManager.Application.Abstractions;
using MotoManager.Domain.Entities;
using MotoManager.Infrastructure.Data;

namespace MotoManager.Infrastructure.Repositories;

public class PurchaseInvoiceRepository : IPurchaseInvoiceRepository
{
    private readonly AppDbContext _context;

    public PurchaseInvoiceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PurchaseInvoice>> GetAllAsync(
        DateTime? datumOd = null, 
        DateTime? datumDo = null, 
        int? dobavljacId = null, 
        int? voziloId = null)
    {
        var query = _context.PurchaseInvoices
            .Include(i => i.Dobavljac)
            .Include(i => i.Vozilo)
            .AsQueryable();
        
        // SQL WHERE clause filtering
        if (datumOd.HasValue)
        {
            query = query.Where(i => i.Datum >= datumOd.Value);
        }
        
        if (datumDo.HasValue)
        {
            query = query.Where(i => i.Datum <= datumDo.Value);
        }
        
        if (dobavljacId.HasValue && dobavljacId.Value > 0)
        {
            query = query.Where(i => i.DobavljacId == dobavljacId.Value);
        }
        
        if (voziloId.HasValue && voziloId.Value > 0)
        {
            query = query.Where(i => i.VoziloId == voziloId.Value);
        }
        
        return await query.ToListAsync();
    }

    public async Task<PurchaseInvoice?> GetByIdAsync(int id)
    {
        return await _context.PurchaseInvoices
            .Include(i => i.Dobavljac)
            .Include(i => i.Vozilo)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<PurchaseInvoice> CreateAsync(PurchaseInvoice purchaseInvoice)
    {
        _context.PurchaseInvoices.Add(purchaseInvoice);
        await _context.SaveChangesAsync();
        
        return await GetByIdAsync(purchaseInvoice.Id) ?? purchaseInvoice;
    }

    public async Task<PurchaseInvoice?> UpdateAsync(PurchaseInvoice purchaseInvoice)
    {
        var existing = await _context.PurchaseInvoices.FindAsync(purchaseInvoice.Id);
        if (existing == null) return null;

        existing.BrojRacuna = purchaseInvoice.BrojRacuna;
        existing.Datum = purchaseInvoice.Datum;
        existing.DobavljacId = purchaseInvoice.DobavljacId;
        existing.VoziloId = purchaseInvoice.VoziloId;
        existing.IznosNeto = purchaseInvoice.IznosNeto;
        existing.IznosPDV = purchaseInvoice.IznosPDV;
        existing.IznosBruto = purchaseInvoice.IznosBruto;

        await _context.SaveChangesAsync();
        
        return await GetByIdAsync(existing.Id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var purchaseInvoice = await _context.PurchaseInvoices.FindAsync(id);
        if (purchaseInvoice == null) return false;

        _context.PurchaseInvoices.Remove(purchaseInvoice);
        await _context.SaveChangesAsync();
        return true;
    }
}
