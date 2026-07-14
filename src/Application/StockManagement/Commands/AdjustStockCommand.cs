using FluentValidation;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.DTOs;
using InventoryManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.StockManagement.Commands;

public record AdjustStockCommand(Guid ProductId, int Quantity, string Reference, bool IsAddition) : IRequest<InventoryDto>;

public class AdjustStockCommandHandler : IRequestHandler<AdjustStockCommand, InventoryDto>
{
    private readonly IApplicationDbContext _context;

    public AdjustStockCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryDto> Handle(AdjustStockCommand request, CancellationToken cancellationToken)
    {
        var inventory = await _context.Inventories
            .Include(i => i.Product)
            .FirstOrDefaultAsync(i => i.ProductId == request.ProductId, cancellationToken)
            ?? throw new KeyNotFoundException($"Inventory for product {request.ProductId} not found.");

        StockMovement movement;
        if (request.IsAddition)
            movement = inventory.AddStock(request.Quantity, request.Reference);
        else
            movement = inventory.RemoveStock(request.Quantity, request.Reference);

        _context.StockMovements.Add(movement);
        await _context.SaveChangesAsync(cancellationToken);

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

public class AdjustStockCommandValidator : AbstractValidator<AdjustStockCommand>
{
    public AdjustStockCommandValidator()
    {
        RuleFor(v => v.ProductId).NotEmpty();
        RuleFor(v => v.Quantity).GreaterThan(0);
        RuleFor(v => v.Reference).NotEmpty().MaximumLength(200);
    }
}
