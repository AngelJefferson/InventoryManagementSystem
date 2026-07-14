using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Products.Queries;

public record GetLowStockProductsQuery(int Threshold = 5) : IRequest<IEnumerable<ProductDto>>;

public class GetLowStockProductsQueryHandler : IRequestHandler<GetLowStockProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IApplicationDbContext _context;

    public GetLowStockProductsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetLowStockProductsQuery request, CancellationToken cancellationToken)
    {
        var lowStockProductIds = await _context.Inventories
            .Where(i => i.QuantityOnHand <= request.Threshold)
            .Select(i => i.ProductId)
            .ToListAsync(cancellationToken);

        return await _context.Products
            .Include(p => p.Category)
            .Where(p => lowStockProductIds.Contains(p.Id))
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                SKU = p.SKU,
                Price = p.Price.Amount,
                Currency = p.Price.Currency,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
}
