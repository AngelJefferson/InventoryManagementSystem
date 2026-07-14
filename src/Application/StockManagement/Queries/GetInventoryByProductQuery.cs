using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.StockManagement.Queries;

public record GetInventoryByProductQuery(Guid ProductId) : IRequest<InventoryDto>;

public class GetInventoryByProductQueryHandler : IRequestHandler<GetInventoryByProductQuery, InventoryDto>
{
    private readonly IApplicationDbContext _context;

    public GetInventoryByProductQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryDto> Handle(GetInventoryByProductQuery request, CancellationToken cancellationToken)
    {
        var inventory = await _context.Inventories
            .Include(i => i.Product)
            .FirstOrDefaultAsync(i => i.ProductId == request.ProductId, cancellationToken)
            ?? throw new KeyNotFoundException($"Inventory for product {request.ProductId} not found.");

        return new InventoryDto
        {
            Id = inventory.Id,
            ProductId = inventory.ProductId,
            ProductName = inventory.Product.Name,
            QuantityOnHand = inventory.QuantityOnHand,
            MinimumStock = inventory.MinimumStock,
            Location = inventory.Location,
            IsLowStock = inventory.IsLowStock(),
            LastUpdated = inventory.LastUpdated
        };
    }
}
