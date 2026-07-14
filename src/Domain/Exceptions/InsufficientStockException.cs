namespace InventoryManagement.Domain.Exceptions;

public class InsufficientStockException : Exception
{
    public Guid ProductId { get; }
    public int CurrentStock { get; }
    public int RequestedQuantity { get; }

    public InsufficientStockException(Guid productId, int currentStock, int requestedQuantity)
        : base($"Insufficient stock for product {productId}. Current: {currentStock}, Requested: {requestedQuantity}")
    {
        ProductId = productId;
        CurrentStock = currentStock;
        RequestedQuantity = requestedQuantity;
    }
}
