namespace InventoryManagement.Domain.Entities;

public enum StockMovementType
{
    In,
    Out
}

public class StockMovement
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;
    public StockMovementType Type { get; private set; }
    public int Quantity { get; private set; }
    public string Reference { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }

    private StockMovement() { }

    public StockMovement(Guid productId, StockMovementType type, int quantity, string reference)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        Type = type;
        Quantity = quantity;
        Reference = reference ?? string.Empty;
        CreatedAt = DateTime.UtcNow;
    }
}
