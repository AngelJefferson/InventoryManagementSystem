using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Employees.Queries;

public record GetEmployeeByIdQuery(Guid Id) : IRequest<EmployeeDto>;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
{
    private readonly IApplicationDbContext _context;

    public GetEmployeeByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .Include(e => e.Products)
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Employee with Id {request.Id} not found.");

        return new EmployeeDto
        {
            Id = employee.Id,
            FullName = employee.FullName,
            Department = employee.Department,
            Sede = employee.Sede,
            Position = employee.Position,
            IsActive = employee.IsActive,
            AssignedEquipmentCount = employee.Products.Count,
            CreatedAt = employee.CreatedAt
        };
    }
}
