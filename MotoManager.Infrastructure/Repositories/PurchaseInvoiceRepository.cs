using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using MotoManager.Application.Abstractions;
using MotoManager.Application.PurchaseInvoices;
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

    public async Task<PagedResult<PurchaseInvoice>> GetAllPagedAsync(
        DateTime? datumOd = null,
        DateTime? datumDo = null,
        int? dobavljacId = null,
        int? voziloId = null,
        int? sektorId = null,
        int pageNumber = 1,
        int pageSize = 20)
    {
        var datumOdParam = new SqlParameter("@DatumOd", (object?)datumOd ?? DBNull.Value);
        var datumDoParam = new SqlParameter("@DatumDo", (object?)datumDo ?? DBNull.Value);
        var dobavljacIdParam = new SqlParameter("@DobavljacId", (object?)dobavljacId ?? DBNull.Value);
        var voziloIdParam = new SqlParameter("@VoziloId", (object?)voziloId ?? DBNull.Value);
        var sektorIdParam = new SqlParameter("@SektorId", (object?)sektorId ?? DBNull.Value);
        var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
        var pageSizeParam = new SqlParameter("@PageSize", pageSize);

        var sql = @"EXEC sp_GetPurchaseInvoicesPaged 
                    @DatumOd, @DatumDo, @DobavljacId, @VoziloId, @SektorId, @PageNumber, @PageSize";

        var connection = _context.Database.GetDbConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.CommandTimeout = 30; // 30 seconds timeout
        command.Parameters.Add(datumOdParam);
        command.Parameters.Add(datumDoParam);
        command.Parameters.Add(dobavljacIdParam);
        command.Parameters.Add(voziloIdParam);
        command.Parameters.Add(sektorIdParam);
        command.Parameters.Add(pageNumberParam);
        command.Parameters.Add(pageSizeParam);

        var invoices = new List<PurchaseInvoice>();
        int totalCount = 0;
        int totalPages = 0;

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var invoice = new PurchaseInvoice
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                BrojRacuna = reader.GetString(reader.GetOrdinal("BrojRacuna")),
                Datum = reader.GetDateTime(reader.GetOrdinal("Datum")),
                DobavljacId = reader.GetInt32(reader.GetOrdinal("DobavljacId")),
                VoziloId = reader.IsDBNull(reader.GetOrdinal("VoziloId")) ? null : reader.GetInt32(reader.GetOrdinal("VoziloId")),
                IznosNeto = reader.GetDecimal(reader.GetOrdinal("IznosNeto")),
                IznosPDV = reader.GetDecimal(reader.GetOrdinal("IznosPDV")),
                IznosBruto = reader.GetDecimal(reader.GetOrdinal("IznosBruto")),
                KorisnikId = reader.IsDBNull(reader.GetOrdinal("KorisnikId")) ? null : reader.GetString(reader.GetOrdinal("KorisnikId")),
                SektorId = reader.IsDBNull(reader.GetOrdinal("SektorId")) ? null : reader.GetInt32(reader.GetOrdinal("SektorId")),
                Dobavljac = new Client
                {
                    Id = reader.GetInt32(reader.GetOrdinal("DobavljacId")),
                    Naziv = reader.IsDBNull(reader.GetOrdinal("DobavljacNaziv")) ? "" : reader.GetString(reader.GetOrdinal("DobavljacNaziv"))
                }
            };

            if (!reader.IsDBNull(reader.GetOrdinal("VoziloId")))
            {
                invoice.Vozilo = new Vehicle
                {
                    Id = reader.GetInt32(reader.GetOrdinal("VoziloId")),
                    Plate = reader.IsDBNull(reader.GetOrdinal("VoziloRegistarskaOznaka")) ? "" : reader.GetString(reader.GetOrdinal("VoziloRegistarskaOznaka")),
                    Model = reader.IsDBNull(reader.GetOrdinal("VoziloModel")) ? "" : reader.GetString(reader.GetOrdinal("VoziloModel"))
                };
            }

            if (!reader.IsDBNull(reader.GetOrdinal("KorisnikId")))
            {
                var imePrezime = reader.IsDBNull(reader.GetOrdinal("KorisnikImePrezime")) ? "" : reader.GetString(reader.GetOrdinal("KorisnikImePrezime"));
                invoice.Korisnik = new Korisnik
                {
                    Id = reader.GetString(reader.GetOrdinal("KorisnikId")),
                    ImePrezime = imePrezime
                };
            }

            if (!reader.IsDBNull(reader.GetOrdinal("SektorId")))
            {
                invoice.Sektor = new Sektor
                {
                    Id = reader.GetInt32(reader.GetOrdinal("SektorId")),
                    Naziv = reader.IsDBNull(reader.GetOrdinal("SektorNaziv")) ? "" : reader.GetString(reader.GetOrdinal("SektorNaziv"))
                };
            }

            totalCount = reader.GetInt32(reader.GetOrdinal("TotalCount"));
            totalPages = Convert.ToInt32(reader.GetDouble(reader.GetOrdinal("TotalPages")));

            invoices.Add(invoice);
        }

        return new PagedResult<PurchaseInvoice>
        {
            Data = invoices,
            TotalCount = totalCount,
            CurrentPage = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages
        };
    }

    public async Task<PurchaseInvoice?> GetByIdAsync(int id)
    {
        return await _context.PurchaseInvoices
            .Include(i => i.Dobavljac)
            .Include(i => i.Vozilo)
            .Include(i => i.Korisnik)
            .Include(i => i.Sektor)
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
        existing.KorisnikId = purchaseInvoice.KorisnikId;
        existing.SektorId = purchaseInvoice.SektorId;

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
