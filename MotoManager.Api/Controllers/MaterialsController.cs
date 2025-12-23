using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MotoManager.Application.Materials;

namespace MotoManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MaterialsController : ControllerBase
{
    private readonly MaterialService _service;

    public MaterialsController(MaterialService service)
    {
        _service = service;
    }

    [HttpGet]
    public async System.Threading.Tasks.Task<ActionResult<System.Collections.Generic.IEnumerable<MaterialDto>>> GetAll()
    {
        var materials = await _service.GetAllAsync();
        return Ok(materials);
    }

    [HttpGet("paged")]
    public async System.Threading.Tasks.Task<ActionResult> GetAllPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _service.GetAllPagedAsync(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async System.Threading.Tasks.Task<ActionResult<MaterialDto>> GetById(int id)
    {
        var material = await _service.GetByIdAsync(id);
        if (material == null)
            return NotFound();
        return Ok(material);
    }

    [HttpPost]
    public async System.Threading.Tasks.Task<ActionResult<MaterialDto>> Create(CreateMaterialRequest request)
    {
        var material = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = material.Id }, material);
    }

    [HttpPut("{id}")]
    public async System.Threading.Tasks.Task<ActionResult<MaterialDto>> Update(int id, UpdateMaterialRequest request)
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
