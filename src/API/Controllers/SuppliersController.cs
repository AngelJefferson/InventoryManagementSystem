using InventoryManagement.Application.Suppliers.Commands;
using InventoryManagement.Application.Suppliers.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly IMediator _mediator;

    public SuppliersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var suppliers = await _mediator.Send(new GetAllSuppliersQuery());
        return Ok(suppliers);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var supplier = await _mediator.Send(new GetSupplierByIdQuery(id));
        return Ok(supplier);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSupplierCommand command)
    {
        var supplier = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = supplier.Id }, supplier);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSupplierCommand command)
    {
        if (id != command.Id)
            return BadRequest("Id mismatch");

        var supplier = await _mediator.Send(command);
        return Ok(supplier);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteSupplierCommand(id));
        return NoContent();
    }
}
