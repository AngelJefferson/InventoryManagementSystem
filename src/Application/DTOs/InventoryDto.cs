namespace InventoryManagement.Application.DTOs;

public class InventoryDto
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int QuantityOnHand { get; init; }
    public int MinimumStock { get; init; }
    public string Location { get; init; } = string.Empty;
    public bool IsLowStock { get; init; }
    public DateTime LastUpdated { get; init; }
}

public class StockMovementDto
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public string Type { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public string Reference { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
