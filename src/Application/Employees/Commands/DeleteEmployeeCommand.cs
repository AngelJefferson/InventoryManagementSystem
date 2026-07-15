using InventoryManagement.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Employees.Commands;

public record DeleteEmployeeCommand(Guid Id) : IRequest<Unit>;

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteEmployeeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Employee with Id {request.Id} not found.");

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
