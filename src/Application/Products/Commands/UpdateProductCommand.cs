using FluentValidation;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Products.Commands;

public record UpdateProductCommand(
    Guid Id,
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

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Product with Id {request.Id} not found.");

        product.SetName(request.Name);
        product.SetDescription(request.Description);
        product.SetSKU(request.SKU);
        product.SetModel(request.Model);
        product.SetAssetNumber(request.AssetNumber);
        product.SetDepartment(request.Department);
        product.SetPhysicalLocation(request.PhysicalLocation);
        product.SetOperatingSystem(request.OperatingSystem);
        product.SetHardwareConfiguration(request.HardwareConfiguration);
        product.SetStatus(request.Status);
        product.SetAcquisitionDate(request.AcquisitionDate);
        product.SetObservations(request.Observations);
        product.SetMaintenanceDate(request.MaintenanceDate);
        product.AssignCategory(request.CategoryId);
        product.AssignSupplier(request.SupplierId);
        product.AssignEmployee(request.EmployeeId);

        await _context.SaveChangesAsync(cancellationToken);

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            SKU = product.SKU,
            Model = product.Model,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty,
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

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
        RuleFor(v => v.Name).NotEmpty().MaximumLength(200);
        RuleFor(v => v.SKU).NotEmpty().MaximumLength(50);
        RuleFor(v => v.CategoryId).NotEmpty();
    }
}
