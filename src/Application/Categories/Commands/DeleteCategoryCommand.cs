using InventoryManagement.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Categories.Commands;

public record DeleteCategoryCommand(Guid Id) : IRequest<Unit>;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Category with Id {request.Id} not found.");

        if (await _context.Products.AnyAsync(p => p.CategoryId == request.Id, cancellationToken))
            throw new InvalidOperationException("Cannot delete category with associated products.");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
