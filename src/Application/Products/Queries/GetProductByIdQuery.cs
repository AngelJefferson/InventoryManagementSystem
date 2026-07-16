using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Products.Queries;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto>;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IApplicationDbContext _context;

    public GetProductByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Include(p => p.Employee)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Product with Id {request.Id} not found.");

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            SKU = product.SKU,
            Model = product.Model,
            CategoryId = product.CategoryId,
            CategoryName = product.Category.Name,
            SupplierId = product.SupplierId,
            SupplierName = product.Supplier?.Name,
            EmployeeId = product.EmployeeId,
            EmployeeName = product.Employee?.FullName,
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
