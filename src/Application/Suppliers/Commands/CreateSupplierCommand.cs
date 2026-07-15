using FluentValidation;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.DTOs;
using InventoryManagement.Domain.Entities;
using MediatR;

namespace InventoryManagement.Application.Suppliers.Commands;

public record CreateSupplierCommand(string Name, string ContactName, string Email, string Phone) : IRequest<SupplierDto>;

public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, SupplierDto>
{
    private readonly IApplicationDbContext _context;

    public CreateSupplierCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SupplierDto> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = new Supplier(request.Name, request.ContactName, request.Email, request.Phone);

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync(cancellationToken);

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

public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
{
    public CreateSupplierCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200);
        RuleFor(v => v.ContactName).MaximumLength(200);
        RuleFor(v => v.Email).MaximumLength(200);
        RuleFor(v => v.Phone).MaximumLength(50);
    }
}
