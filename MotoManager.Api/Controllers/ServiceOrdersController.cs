using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MotoManager.Application.ServiceOrders;

namespace MotoManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ServiceOrdersController : ControllerBase
{
    private readonly ServiceOrderService _serviceOrderService;

    public ServiceOrdersController(ServiceOrderService serviceOrderService)
    {
        _serviceOrderService = serviceOrderService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceOrderDto>>> GetAll()
    {
        var orders = await _serviceOrderService.GetAllServiceOrdersAsync();
        return Ok(orders);
    }

    [HttpGet("paged")]
    public async Task<ActionResult> GetAllPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _serviceOrderService.GetAllServiceOrdersPagedAsync(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceOrderDto>> GetById(int id)
    {
        var order = await _serviceOrderService.GetServiceOrderByIdAsync(id);
        if (order == null)
            return NotFound();
        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceOrderDto>> Create([FromBody] CreateServiceOrderRequest request)
    {
        // Automatski postavi KorisnikId iz JWT tokena
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                     ?? User.FindFirst("sub")?.Value;
        
        if (!string.IsNullOrEmpty(userId))
        {
            request = request with { KorisnikId = userId };
        }
        
        var order = await _serviceOrderService.CreateServiceOrderAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceOrderDto>> Update(int id, [FromBody] UpdateServiceOrderRequest request)
    {
        if (id != request.Id)
            return BadRequest();

        // Automatski postavi KorisnikId iz JWT tokena
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                     ?? User.FindFirst("sub")?.Value;
        
        if (!string.IsNullOrEmpty(userId))
        {
            request = request with { KorisnikId = userId };
        }

        var order = await _serviceOrderService.UpdateServiceOrderAsync(request);
        if (order == null)
            return NotFound();

        return Ok(order);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _serviceOrderService.DeleteServiceOrderAsync(id);
        if (!success)
            return NotFound();
        return NoContent();
    }
}
