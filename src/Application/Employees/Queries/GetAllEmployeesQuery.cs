using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Employees.Queries;

public record GetAllEmployeesQuery : IRequest<IEnumerable<EmployeeDto>>;

public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, IEnumerable<EmployeeDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllEmployeesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EmployeeDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Employees
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                FullName = e.FullName,
                Department = e.Department,
                Sede = e.Sede,
                Position = e.Position,
                IsActive = e.IsActive,
                AssignedEquipmentCount = e.Products.Count,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
}
