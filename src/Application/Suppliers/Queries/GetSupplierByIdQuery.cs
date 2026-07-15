using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Suppliers.Queries;

public record GetSupplierByIdQuery(Guid Id) : IRequest<SupplierDto>;

public class GetSupplierByIdQueryHandler : IRequestHandler<GetSupplierByIdQuery, SupplierDto>
{
    private readonly IApplicationDbContext _context;

    public GetSupplierByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SupplierDto> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
    {
        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Supplier with Id {request.Id} not found.");

        return new SupplierDto
        {
            Id = supplier.Id,
            Name = supplier.Name,
            ContactName = supplier.ContactName,
            Email = supplier.Email,
            Phone = supplier.Phone,
            CreatedAt = supplier.CreatedAt
        };
    }
}
