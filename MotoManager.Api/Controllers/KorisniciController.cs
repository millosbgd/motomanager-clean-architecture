using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MotoManager.Application.Korisnici;

namespace MotoManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class KorisniciController : ControllerBase
{
    private readonly KorisnikService _korisnikService;

    public KorisniciController(KorisnikService korisnikService)
    {
        _korisnikService = korisnikService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<KorisnikDto>>> GetAll()
    {
        var korisnici = await _korisnikService.GetAllKorisniciAsync();
        return Ok(korisnici);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<KorisnikDto>> GetById(string id)
    {
        var korisnik = await _korisnikService.GetKorisnikByIdAsync(id);
        if (korisnik == null)
            return NotFound();
        return Ok(korisnik);
    }

    [HttpGet("by-username/{userName}")]
    public async Task<ActionResult<KorisnikDto>> GetByUserName(string userName)
    {
        var korisnik = await _korisnikService.GetKorisnikByUserNameAsync(userName);
        if (korisnik == null)
            return NotFound();
        return Ok(korisnik);
    }

    [HttpGet("exists/{id}")]
    public async Task<ActionResult<bool>> Exists(string id)
    {
        var exists = await _korisnikService.KorisnikExistsAsync(id);
        return Ok(exists);
    }

    [HttpPost]
    public async Task<ActionResult<KorisnikDto>> Create([FromBody] CreateKorisnikRequest request)
    {
        var korisnik = await _korisnikService.CreateKorisnikAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = korisnik.Id }, korisnik);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<KorisnikDto>> Update(string id, [FromBody] UpdateKorisnikRequest request)
    {
        if (id != request.Id)
            return BadRequest();

        var korisnik = await _korisnikService.UpdateKorisnikAsync(request);
        if (korisnik == null)
            return NotFound();

        return Ok(korisnik);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var success = await _korisnikService.DeleteKorisnikAsync(id);
        if (!success)
            return NotFound();
        return NoContent();
    }
}
