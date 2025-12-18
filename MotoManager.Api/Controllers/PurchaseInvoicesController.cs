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

    [HttpGet("export")]
    public async Task<IActionResult> ExportToExcel(
        [FromQuery] DateTime? datumOd = null,
        [FromQuery] DateTime? datumDo = null,
        [FromQuery] int? dobavljacId = null,
        [FromQuery] int? voziloId = null)
    {
        var excelData = await _purchaseInvoiceService.ExportToExcelAsync(datumOd, datumDo, dobavljacId, voziloId);
        
        var fileName = $"Racuni_dobavljaca_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
        return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }
}
