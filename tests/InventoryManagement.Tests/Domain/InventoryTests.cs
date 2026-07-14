using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;

namespace InventoryManagement.Tests.Domain;

public class InventoryTests
{
    [Fact]
    public void AddStock_IncreasesQuantity()
    {
        var inventory = new Inventory(Guid.NewGuid(), 10);

        inventory.AddStock(5, "test");

        Assert.Equal(15, inventory.QuantityOnHand);
    }

    [Fact]
    public void RemoveStock_DecreasesQuantity()
    {
        var inventory = new Inventory(Guid.NewGuid(), 10);

        inventory.RemoveStock(3, "test");

        Assert.Equal(7, inventory.QuantityOnHand);
    }

    [Fact]
    public void RemoveStock_InsufficientStock_Throws()
    {
        var inventory = new Inventory(Guid.NewGuid(), 5);

        Assert.Throws<InsufficientStockException>(() =>
            inventory.RemoveStock(10, "test"));
    }

    [Fact]
    public void AddStock_WithNegativeQuantity_Throws()
    {
        var inventory = new Inventory(Guid.NewGuid(), 10);

        Assert.Throws<ArgumentException>(() =>
            inventory.AddStock(-1, "test"));
    }

    [Fact]
    public void IsLowStock_WhenBelowThreshold_ReturnsTrue()
    {
        var inventory = new Inventory(Guid.NewGuid(), 3, minimumStock: 5);

        Assert.True(inventory.IsLowStock());
    }

    [Fact]
    public void IsLowStock_WhenAboveThreshold_ReturnsFalse()
    {
        var inventory = new Inventory(Guid.NewGuid(), 10, minimumStock: 5);

        Assert.False(inventory.IsLowStock());
    }
}
