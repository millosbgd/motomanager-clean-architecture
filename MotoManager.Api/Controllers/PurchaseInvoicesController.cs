using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MotoManager.Application.PurchaseInvoices;

namespace MotoManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PurchaseInvoicesController : ControllerBase
{
    private readonly PurchaseInvoiceService _purchaseInvoiceService;

    public PurchaseInvoicesController(PurchaseInvoiceService purchaseInvoiceService)
    {
        _purchaseInvoiceService = purchaseInvoiceService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PurchaseInvoiceDto>>> GetAll(
        [FromQuery] DateTime? datumOd = null,
        [FromQuery] DateTime? datumDo = null,
        [FromQuery] int? dobavljacId = null,
        [FromQuery] int? voziloId = null)
    {
        var invoices = await _purchaseInvoiceService.GetAllPurchaseInvoicesAsync(datumOd, datumDo, dobavljacId, voziloId);
        return Ok(invoices);
    }

    [HttpGet("export")]
    public async Task<IActionResult> ExportToExcel(
        [FromQuery] string? datumOd = null,
        [FromQuery] string? datumDo = null,
        [FromQuery] string? dobavljacId = null,
        [FromQuery] string? voziloId = null)
    {
        try
        {
            DateTime? parsedDatumOd = null;
            DateTime? parsedDatumDo = null;
            int? parsedDobavljacId = null;
            int? parsedVoziloId = null;
            var format = "yyyy-MM-dd";
            var culture = System.Globalization.CultureInfo.InvariantCulture;

            if (!string.IsNullOrWhiteSpace(datumOd))
            {
                if (DateTime.TryParseExact(datumOd, format, culture, System.Globalization.DateTimeStyles.None, out var tempOd))
                    parsedDatumOd = tempOd;
            }

            if (!string.IsNullOrWhiteSpace(datumDo))
            {
                if (DateTime.TryParseExact(datumDo, format, culture, System.Globalization.DateTimeStyles.None, out var tempDo))
                    parsedDatumDo = tempDo;
            }

            if (!string.IsNullOrWhiteSpace(dobavljacId))
            {
                if (int.TryParse(dobavljacId, out var tempDobavljacId))
                    parsedDobavljacId = tempDobavljacId;
            }

            if (!string.IsNullOrWhiteSpace(voziloId))
            {
                if (int.TryParse(voziloId, out var tempVoziloId))
                    parsedVoziloId = tempVoziloId;
            }

            var excelData = await _purchaseInvoiceService.ExportToExcelAsync(parsedDatumOd, parsedDatumDo, parsedDobavljacId, parsedVoziloId);
            var fileName = $"Racuni_dobavljaca_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch (Exception ex)
        {
            // DEBUG: Vraćam detaljnu poruku
            return BadRequest($"Export error: {ex.Message}\n{ex.StackTrace}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PurchaseInvoiceDto>> GetById(int id)
    {
        var invoice = await _purchaseInvoiceService.GetPurchaseInvoiceByIdAsync(id);
        if (invoice == null)
            return NotFound();
        return Ok(invoice);
    }

    [HttpPost]
    public async Task<ActionResult<PurchaseInvoiceDto>> Create([FromBody] CreatePurchaseInvoiceRequest request)
    {
        var invoice = await _purchaseInvoiceService.CreatePurchaseInvoiceAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, invoice);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PurchaseInvoiceDto>> Update(int id, [FromBody] UpdatePurchaseInvoiceRequest request)
    {
        if (id != request.Id)
            return BadRequest();

        var invoice = await _purchaseInvoiceService.UpdatePurchaseInvoiceAsync(request);
        if (invoice == null)
            return NotFound();

        return Ok(invoice);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _purchaseInvoiceService.DeletePurchaseInvoiceAsync(id);
        if (!success)
            return NotFound();
        return NoContent();
    }
}
