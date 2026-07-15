using FluentValidation;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Suppliers.Commands;

public record UpdateSupplierCommand(Guid Id, string Name, string ContactName, string Email, string Phone) : IRequest<SupplierDto>;

public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand, SupplierDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateSupplierCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SupplierDto> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Supplier with Id {request.Id} not found.");

        supplier.SetName(request.Name);
        supplier.SetContactName(request.ContactName);
        supplier.SetEmail(request.Email);
        supplier.SetPhone(request.Phone);

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

public class UpdateSupplierCommandValidator : AbstractValidator<UpdateSupplierCommand>
{
    public UpdateSupplierCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
        RuleFor(v => v.Name).NotEmpty().MaximumLength(200);
        RuleFor(v => v.ContactName).MaximumLength(200);
        RuleFor(v => v.Email).MaximumLength(200);
        RuleFor(v => v.Phone).MaximumLength(50);
    }
}
