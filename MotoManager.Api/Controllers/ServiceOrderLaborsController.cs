using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MotoManager.Application.ServiceOrderLabors;

namespace MotoManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ServiceOrderLaborsController : ControllerBase
{
    private readonly ServiceOrderLaborService _service;

    public ServiceOrderLaborsController(ServiceOrderLaborService service)
    {
        _service = service;
    }

    [HttpGet("service-order/{serviceOrderId}")]
    public async System.Threading.Tasks.Task<ActionResult<System.Collections.Generic.IEnumerable<ServiceOrderLaborDto>>> GetByServiceOrderId(int serviceOrderId)
    {
        var labors = await _service.GetAllByServiceOrderIdAsync(serviceOrderId);
        return Ok(labors);
    }

    [HttpGet("{id}")]
    public async System.Threading.Tasks.Task<ActionResult<ServiceOrderLaborDto>> GetById(int id)
    {
        var labor = await _service.GetByIdAsync(id);
        if (labor == null)
            return NotFound();
        return Ok(labor);
    }

    [HttpPost]
    public async System.Threading.Tasks.Task<ActionResult<ServiceOrderLaborDto>> Create(CreateServiceOrderLaborRequest request)
    {
        var labor = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = labor.Id }, labor);
    }

    [HttpPut("{id}")]
    public async System.Threading.Tasks.Task<ActionResult<ServiceOrderLaborDto>> Update(int id, UpdateServiceOrderLaborRequest request)
    {
        if (id != request.Id)
            return BadRequest();

        var labor = await _service.UpdateAsync(request);
        return Ok(labor);
    }

    [HttpDelete("{id}")]
    public async System.Threading.Tasks.Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
