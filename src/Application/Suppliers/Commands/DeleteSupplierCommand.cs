using InventoryManagement.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Suppliers.Commands;

public record DeleteSupplierCommand(Guid Id) : IRequest<Unit>;

public class DeleteSupplierCommandHandler : IRequestHandler<DeleteSupplierCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteSupplierCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Supplier with Id {request.Id} not found.");

        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
