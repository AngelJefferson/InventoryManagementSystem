using InventoryManagement.Domain.Exceptions;

namespace InventoryManagement.Domain.Entities;

public class Inventory
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;
    public int QuantityOnHand { get; private set; }
    public int MinimumStock { get; private set; }
    public string Location { get; private set; } = null!;
    public DateTime LastUpdated { get; private set; }

    private Inventory() { }

    public Inventory(Guid productId, int initialQuantity, int minimumStock = 0, string location = "Main Warehouse")
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        MinimumStock = minimumStock;
        Location = location;
        LastUpdated = DateTime.UtcNow;

        if (initialQuantity < 0)
            throw new ArgumentException("Initial quantity cannot be negative", nameof(initialQuantity));
        QuantityOnHand = initialQuantity;
    }

    public StockMovement AddStock(int quantity, string reference)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        QuantityOnHand += quantity;
        LastUpdated = DateTime.UtcNow;
        return new StockMovement(ProductId, StockMovementType.In, quantity, reference);
    }

    public StockMovement RemoveStock(int quantity, string reference)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        if (QuantityOnHand - quantity < 0)
            throw new InsufficientStockException(ProductId, QuantityOnHand, quantity);

        QuantityOnHand -= quantity;
        LastUpdated = DateTime.UtcNow;
        return new StockMovement(ProductId, StockMovementType.Out, quantity, reference);
    }

    public bool IsLowStock() => QuantityOnHand <= MinimumStock;
}
