namespace InventoryManagement.Application.DTOs;

public class ProductDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string SKU { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public Guid CategoryId { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public Guid? SupplierId { get; init; }
    public string? SupplierName { get; init; }
    public Guid? EmployeeId { get; init; }
    public string? EmployeeName { get; init; }
    public string? AssetNumber { get; init; }
    public string Department { get; init; } = string.Empty;
    public string PhysicalLocation { get; init; } = string.Empty;
    public string OperatingSystem { get; init; } = string.Empty;
    public string HardwareConfiguration { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime? AcquisitionDate { get; init; }
    public string Observations { get; init; } = string.Empty;
    public DateTime? MaintenanceDate { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
}
