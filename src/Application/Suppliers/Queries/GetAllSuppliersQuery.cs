using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Suppliers.Queries;

public record GetAllSuppliersQuery : IRequest<IEnumerable<SupplierDto>>;

public class GetAllSuppliersQueryHandler : IRequestHandler<GetAllSuppliersQuery, IEnumerable<SupplierDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllSuppliersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SupplierDto>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Suppliers
            .Select(s => new SupplierDto
            {
                Id = s.Id,
                Name = s.Name,
                ContactName = s.ContactName,
                Email = s.Email,
                Phone = s.Phone,
                CreatedAt = s.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
}
