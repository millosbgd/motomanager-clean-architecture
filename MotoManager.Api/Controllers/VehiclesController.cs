using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MotoManager.Application.Vehicles;

namespace MotoManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VehiclesController : ControllerBase
{
    private readonly VehicleService _service;

    public VehiclesController(VehicleService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<VehicleDto>>> GetAll()
    {
        var vehicles = await _service.GetAllAsync();
        return Ok(vehicles);
    }

    [HttpGet("paged")]
    public async Task<ActionResult> GetAllPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _service.GetAllPagedAsync(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VehicleDto>> GetById(int id)
    {
        var v = await _service.GetByIdAsync(id);
        if (v is null) return NotFound();
        return Ok(v);
    }

    [HttpPost]
    public async Task<ActionResult<VehicleDto>> Create(CreateVehicleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Model) || string.IsNullOrWhiteSpace(request.Plate) || request.ClientId <= 0)
            return BadRequest("Model, tablica i klijent su obavezni.");

        var created = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateVehicleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Model) || string.IsNullOrWhiteSpace(request.Plate) || request.ClientId <= 0)
            return BadRequest("Model, tablica i klijent su obavezni.");

        var updateRequest = new UpdateVehicleRequest(id, request.Model, request.Plate, request.ClientId);
        var ok = await _service.UpdateAsync(updateRequest);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
