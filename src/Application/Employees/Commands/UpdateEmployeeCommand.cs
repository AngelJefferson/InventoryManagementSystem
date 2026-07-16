using FluentValidation;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Employees.Commands;

public record UpdateEmployeeCommand(Guid Id, string FullName, string Department, string Sede, string Position, string Email) : IRequest<EmployeeDto>;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateEmployeeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EmployeeDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Employee with Id {request.Id} not found.");

        employee.SetFullName(request.FullName);
        employee.SetDepartment(request.Department);
        employee.SetSede(request.Sede);
        employee.SetPosition(request.Position);
        employee.SetEmail(request.Email);

        await _context.SaveChangesAsync(cancellationToken);

        return new EmployeeDto
        {
            Id = employee.Id,
            FullName = employee.FullName,
            Department = employee.Department,
            Sede = employee.Sede,
            Position = employee.Position,
            Email = employee.Email,
            IsActive = employee.IsActive,
            CreatedAt = employee.CreatedAt
        };
    }
}

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
        RuleFor(v => v.FullName)
            .NotEmpty().WithMessage("El nombre completo es obligatorio.")
            .MaximumLength(200);
        RuleFor(v => v.Department).MaximumLength(100);
        RuleFor(v => v.Sede).MaximumLength(100);
        RuleFor(v => v.Position).MaximumLength(100);
        RuleFor(v => v.Email).MaximumLength(200);
    }
}
