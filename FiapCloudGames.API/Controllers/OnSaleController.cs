using FiapCloudGames.Application.DTOs.OnSale.Request;
using FiapCloudGames.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OnSaleController : ControllerBase
{
    private readonly IOnSaleService _service;

    public OnSaleController (IOnSaleService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("[action]")]
    [SwaggerOperation(Summary = "Get all On Sales.")]

    public async Task<IActionResult> GetAllOnSale ()
     => Ok(await _service.GetAllAsync());

    [HttpGet]
    [Route("[action]")]
    [SwaggerOperation(Summary = "Search for on sales by ID.")]
    public async Task<IActionResult> GetOnSaleById ([FromQuery] Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [Route("[action]")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Create a new on sales.")]

    public async Task<IActionResult> CreateOnSale ([FromBody] OnSaleRequest request)
    {
        var created = await _service.CreateAsync(request);
        if (created == null) return NotFound(new ProblemDetails
        {
            Title = "Jogo informado não foi localizado!",
            Status = StatusCodes.Status404NotFound
        });

        return CreatedAtAction(nameof(GetOnSaleById), new { id = created.Id }, created);
    }

    [HttpPut]
    [Route("[action]")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Update a on sales.")]
    public async Task<IActionResult> UpdateOnSale (Guid id, [FromBody] OnSaleRequest request)
    {
        var updated = await _service.UpdateAsync(id, request);
        return Ok(updated);
    }


}
