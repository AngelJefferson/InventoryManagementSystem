using FluentValidation;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.DTOs;
using InventoryManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Products.Commands;

public record BulkCreateProductItem(
    string Name,
    string CategoryName,
    string Model,
    string SKU,
    string? AssetNumber,
    string? EmployeeName,
    string Department,
    string PhysicalLocation,
    string OperatingSystem,
    string HardwareConfiguration,
    string Status,
    DateTime? AcquisitionDate,
    string Observations,
    DateTime? MaintenanceDate
);

public record BulkCreateProductsCommand(List<BulkCreateProductItem> Products) : IRequest<int>;

public class BulkCreateProductsCommandHandler : IRequestHandler<BulkCreateProductsCommand, int>
{
    private readonly IApplicationDbContext _context;

    public BulkCreateProductsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(BulkCreateProductsCommand request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories.ToListAsync(cancellationToken);
        var employees = await _context.Employees.ToListAsync(cancellationToken);
        var created = 0;

        foreach (var item in request.Products)
        {
            var category = categories.FirstOrDefault(c => c.Name.Equals(item.CategoryName, StringComparison.OrdinalIgnoreCase));
            if (category == null)
            {
                category = new Category(item.CategoryName, "");
                _context.Categories.Add(category);
                categories.Add(category);
            }

            Guid? employeeId = null;
            if (!string.IsNullOrWhiteSpace(item.EmployeeName))
            {
                var employee = employees.FirstOrDefault(e => e.FullName.Equals(item.EmployeeName, StringComparison.OrdinalIgnoreCase));
                if (employee == null)
                {
                    employee = new Employee(item.EmployeeName, item.Department, item.PhysicalLocation, "", "");
                    _context.Employees.Add(employee);
                    employees.Add(employee);
                }
                employeeId = employee.Id;
            }

            var product = new Product(
                item.Name, "", item.SKU, category.Id,
                null, item.Model, employeeId,
                item.AssetNumber, item.Department, item.PhysicalLocation,
                item.OperatingSystem, item.HardwareConfiguration,
                item.Status, item.AcquisitionDate, item.Observations, item.MaintenanceDate
            );

            _context.Products.Add(product);
            created++;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return created;
    }
}
