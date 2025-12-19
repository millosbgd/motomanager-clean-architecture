using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MotoManager.Application.Sektori;

namespace MotoManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SektoriController : ControllerBase
{
    private readonly SektorService _sektorService;

    public SektoriController(SektorService sektorService)
    {
        _sektorService = sektorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SektorDto>>> GetAll()
    {
        var sektori = await _sektorService.GetAllSektoriAsync();
        return Ok(sektori);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SektorDto>> GetById(int id)
    {
        var sektor = await _sektorService.GetSektorByIdAsync(id);
        if (sektor == null)
            return NotFound();
        return Ok(sektor);
    }

    [HttpPost]
    public async Task<ActionResult<SektorDto>> Create([FromBody] CreateSektorRequest request)
    {
        var sektor = await _sektorService.CreateSektorAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = sektor.Id }, sektor);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SektorDto>> Update(int id, [FromBody] UpdateSektorRequest request)
    {
        if (id != request.Id)
            return BadRequest();

        var sektor = await _sektorService.UpdateSektorAsync(request);
        if (sektor == null)
            return NotFound();

        return Ok(sektor);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _sektorService.DeleteSektorAsync(id);
        if (!success)
            return NotFound();
        return NoContent();
    }
}
