using FluentValidation;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.DTOs;
using InventoryManagement.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Products.Commands;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    string SKU,
    string Model,
    decimal Price,
    string Currency,
    Guid CategoryId,
    Guid? SupplierId
) : IRequest<ProductDto>;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Product with Id {request.Id} not found.");

        product.SetName(request.Name);
        product.SetDescription(request.Description);
        product.SetSKU(request.SKU);
        product.SetModel(request.Model);
        product.SetPrice(new Money(request.Price, request.Currency));
        product.AssignCategory(request.CategoryId);
        product.AssignSupplier(request.SupplierId);

        await _context.SaveChangesAsync(cancellationToken);

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            SKU = product.SKU,
            Model = product.Model,
            Price = product.Price.Amount,
            Currency = product.Price.Currency,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty,
            SupplierId = product.SupplierId,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt
        };
    }
}

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
        RuleFor(v => v.Name).NotEmpty().MaximumLength(200);
        RuleFor(v => v.SKU).NotEmpty().MaximumLength(50);
        RuleFor(v => v.Price).GreaterThanOrEqualTo(0);
        RuleFor(v => v.CategoryId).NotEmpty();
    }
}
