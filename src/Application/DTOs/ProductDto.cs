namespace InventoryManagement.Application.DTOs;

public class ProductDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string SKU { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Currency { get; init; } = "USD";
    public Guid CategoryId { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public Guid? SupplierId { get; init; }
    public string? SupplierName { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
}
