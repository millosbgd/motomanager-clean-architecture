using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MotoManager.Application.ServiceOrderMaterials;

namespace MotoManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ServiceOrderMaterialsController : ControllerBase
{
    private readonly ServiceOrderMaterialService _service;

    public ServiceOrderMaterialsController(ServiceOrderMaterialService service)
    {
        _service = service;
    }

    [HttpGet("service-order/{serviceOrderId}")]
    public async System.Threading.Tasks.Task<ActionResult<System.Collections.Generic.IEnumerable<ServiceOrderMaterialDto>>> GetByServiceOrderId(int serviceOrderId)
    {
        var materials = await _service.GetAllByServiceOrderIdAsync(serviceOrderId);
        return Ok(materials);
    }

    [HttpGet("{id}")]
    public async System.Threading.Tasks.Task<ActionResult<ServiceOrderMaterialDto>> GetById(int id)
    {
        var material = await _service.GetByIdAsync(id);
        if (material == null)
            return NotFound();
        return Ok(material);
    }

    [HttpPost]
    public async System.Threading.Tasks.Task<ActionResult<ServiceOrderMaterialDto>> Create(CreateServiceOrderMaterialRequest request)
    {
        var material = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = material.Id }, material);
    }

    [HttpPut("{id}")]
    public async System.Threading.Tasks.Task<ActionResult<ServiceOrderMaterialDto>> Update(int id, UpdateServiceOrderMaterialRequest request)
    {
        if (id != request.Id)
            return BadRequest();

        var material = await _service.UpdateAsync(request);
        return Ok(material);
    }

    [HttpDelete("{id}")]
    public async System.Threading.Tasks.Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
