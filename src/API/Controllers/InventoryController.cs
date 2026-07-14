using InventoryManagement.Application.StockManagement.Commands;
using InventoryManagement.Application.StockManagement.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> GetByProduct(Guid productId)
    {
        var inventory = await _mediator.Send(new GetInventoryByProductQuery(productId));
        return Ok(inventory);
    }

    [HttpGet("{productId:guid}/movements")]
    public async Task<IActionResult> GetMovements(Guid productId)
    {
        var movements = await _mediator.Send(new GetMovementsByProductQuery(productId));
        return Ok(movements);
    }

    [HttpPost("adjust")]
    public async Task<IActionResult> AdjustStock([FromBody] AdjustStockCommand command)
    {
        var inventory = await _mediator.Send(command);
        return Ok(inventory);
    }
}
