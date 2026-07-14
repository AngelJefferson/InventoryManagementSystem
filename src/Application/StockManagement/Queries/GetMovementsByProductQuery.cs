using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.StockManagement.Queries;

public record GetMovementsByProductQuery(Guid ProductId) : IRequest<IEnumerable<StockMovementDto>>;

public class GetMovementsByProductQueryHandler : IRequestHandler<GetMovementsByProductQuery, IEnumerable<StockMovementDto>>
{
    private readonly IApplicationDbContext _context;

    public GetMovementsByProductQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StockMovementDto>> Handle(GetMovementsByProductQuery request, CancellationToken cancellationToken)
    {
        return await _context.StockMovements
            .Where(m => m.ProductId == request.ProductId)
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new StockMovementDto
            {
                Id = m.Id,
                ProductId = m.ProductId,
                Type = m.Type.ToString(),
                Quantity = m.Quantity,
                Reference = m.Reference,
                CreatedAt = m.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
}
