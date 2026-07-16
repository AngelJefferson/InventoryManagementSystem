using FluentValidation;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.DTOs;
using InventoryManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Products.Commands;

public record CreateProductCommand(
    string Name,
    string Description,
    string SKU,
    string Model,
    Guid CategoryId,
    Guid? SupplierId,
    Guid? EmployeeId,
    string? AssetNumber = null,
    string Department = "",
    string PhysicalLocation = "",
    string OperatingSystem = "",
    string HardwareConfiguration = "",
    string Status = "",
    DateTime? AcquisitionDate = null,
    string Observations = "",
    DateTime? MaintenanceDate = null
) : IRequest<ProductDto>;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IApplicationDbContext _context;

    public CreateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (!categoryExists)
            throw new KeyNotFoundException($"Category with Id {request.CategoryId} not found.");

        if (request.SupplierId.HasValue)
        {
            var supplierExists = await _context.Suppliers.AnyAsync(s => s.Id == request.SupplierId.Value, cancellationToken);
            if (!supplierExists)
                throw new KeyNotFoundException($"Supplier with Id {request.SupplierId} not found.");
        }

        var product = new Product(
            request.Name, request.Description, request.SKU, request.CategoryId,
            request.SupplierId, request.Model, request.EmployeeId,
            request.AssetNumber, request.Department, request.PhysicalLocation,
            request.OperatingSystem, request.HardwareConfiguration,
            request.Status, request.AcquisitionDate, request.Observations, request.MaintenanceDate
        );

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            SKU = product.SKU,
            Model = product.Model,
            CategoryId = product.CategoryId,
            SupplierId = product.SupplierId,
            AssetNumber = product.AssetNumber,
            Department = product.Department,
            PhysicalLocation = product.PhysicalLocation,
            OperatingSystem = product.OperatingSystem,
            HardwareConfiguration = product.HardwareConfiguration,
            Status = product.Status,
            AcquisitionDate = product.AcquisitionDate,
            Observations = product.Observations,
            MaintenanceDate = product.MaintenanceDate,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt
        };
    }
}

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty().MaximumLength(200);
        RuleFor(v => v.SKU).NotEmpty().MaximumLength(50);
        RuleFor(v => v.CategoryId).NotEmpty();
        RuleFor(v => v.Model).MaximumLength(200);
    }
}
