using MotoManager.Application.Abstractions;
using MotoManager.Domain.Entities;
using ClosedXML.Excel;

namespace MotoManager.Application.PurchaseInvoices;

public class PurchaseInvoiceService
{
    private readonly IPurchaseInvoiceRepository _purchaseInvoiceRepository;

    public PurchaseInvoiceService(IPurchaseInvoiceRepository purchaseInvoiceRepository)
    {
        _purchaseInvoiceRepository = purchaseInvoiceRepository;
    }

    public async Task<IEnumerable<PurchaseInvoiceDto>> GetAllPurchaseInvoicesAsync(
        DateTime? datumOd = null, 
        DateTime? datumDo = null, 
        int? dobavljacId = null, 
        int? voziloId = null)
    {
        // Filtering happens in Repository (SQL WHERE clause)
        var invoices = await _purchaseInvoiceRepository.GetAllAsync(datumOd, datumDo, dobavljacId, voziloId) 
            ?? Enumerable.Empty<PurchaseInvoice>();
        
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

    public async Task<byte[]> ExportToExcelAsync(
        DateTime? datumOd = null, 
        DateTime? datumDo = null, 
        int? dobavljacId = null, 
        int? voziloId = null)
    {
        var invoices = await _purchaseInvoiceRepository.GetAllAsync(datumOd, datumDo, dobavljacId, voziloId);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Računi dobavljača");

        // Zaglavlje
        worksheet.Cell(1, 1).Value = "Broj računa";
        worksheet.Cell(1, 2).Value = "Datum";
        worksheet.Cell(1, 3).Value = "Dobavljač";
        worksheet.Cell(1, 4).Value = "Vozilo";
        worksheet.Cell(1, 5).Value = "Iznos neto (RSD)";
        worksheet.Cell(1, 6).Value = "PDV (RSD)";
        worksheet.Cell(1, 7).Value = "Bruto iznos (RSD)";

        // Formatiranje zaglavlja
        var headerRange = worksheet.Range("A1:G1");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

        // Podaci
        int row = 2;
        foreach (var invoice in invoices)
        {
            worksheet.Cell(row, 1).Value = invoice.BrojRacuna;
            worksheet.Cell(row, 2).Value = invoice.Datum.ToString("dd.MM.yyyy");
            var dobavljacNaziv = invoice.Dobavljac?.Naziv ?? "-";
            worksheet.Cell(row, 3).Value = dobavljacNaziv;
            worksheet.Cell(row, 4).Value = invoice.Vozilo != null 
                ? $"{invoice.Vozilo.Model} ({invoice.Vozilo.Plate})" 
                : "-";
            worksheet.Cell(row, 5).Value = invoice.IznosNeto;
            worksheet.Cell(row, 6).Value = invoice.IznosPDV;
            worksheet.Cell(row, 7).Value = invoice.IznosBruto;

            // Formatiranje brojeva
            worksheet.Cell(row, 5).Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell(row, 6).Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell(row, 7).Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell(row, 7).Style.Font.Bold = true;

            row++;
        }

        // Ukupni zbir
        if (invoices.Any())
        {
            worksheet.Cell(row, 4).Value = "UKUPNO:";
            worksheet.Cell(row, 4).Style.Font.Bold = true;
            worksheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            
            worksheet.Cell(row, 5).FormulaA1 = $"=SUM(E2:E{row - 1})";
            worksheet.Cell(row, 6).FormulaA1 = $"=SUM(F2:F{row - 1})";
            worksheet.Cell(row, 7).FormulaA1 = $"=SUM(G2:G{row - 1})";

            worksheet.Cell(row, 5).Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell(row, 6).Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell(row, 7).Style.NumberFormat.Format = "#,##0.00";
            
            var totalRange = worksheet.Range($"D{row}:G{row}");
            totalRange.Style.Font.Bold = true;
            totalRange.Style.Fill.BackgroundColor = XLColor.LightYellow;
            totalRange.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        }

        // Podešavanje širine kolona
        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
