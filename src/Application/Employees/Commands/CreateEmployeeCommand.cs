using FluentValidation;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.DTOs;
using InventoryManagement.Domain.Entities;
using MediatR;

namespace InventoryManagement.Application.Employees.Commands;

public record CreateEmployeeCommand(string FullName, string Department, string Sede, string Position, string Email = "") : IRequest<EmployeeDto>;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    private readonly IApplicationDbContext _context;

    public CreateEmployeeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = new Employee(request.FullName, request.Department, request.Sede, request.Position, request.Email);

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync(cancellationToken);

        return new EmployeeDto
        {
            Id = employee.Id,
            FullName = employee.FullName,
            Department = employee.Department,
            Sede = employee.Sede,
            Position = employee.Position,
            IsActive = employee.IsActive,
            CreatedAt = employee.CreatedAt
        };
    }
}

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(v => v.FullName)
            .NotEmpty().WithMessage("El nombre completo es obligatorio.")
            .MaximumLength(200).WithMessage("El nombre no debe exceder 200 caracteres.");

        RuleFor(v => v.Department).MaximumLength(100);
        RuleFor(v => v.Sede).MaximumLength(100);
        RuleFor(v => v.Position).MaximumLength(100);
    }
}
