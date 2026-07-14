using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.StockManagement.Commands;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Infrastructure.Data;

namespace InventoryManagement.Tests.Application;

public class AdjustStockCommandTests
{
    private static async Task<(IApplicationDbContext context, Guid productId)> SeedData()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        var category = new Category("Test", "desc");
        context.Categories.Add(category);
        await context.SaveChangesAsync(default);

        var product = new Product("Test Product", "desc", "TST-001",
            new Money(100m), category.Id);
        context.Products.Add(product);
        await context.SaveChangesAsync(default);

        var inventory = new Inventory(product.Id, 10);
        context.Inventories.Add(inventory);
        await context.SaveChangesAsync(default);

        return (context, product.Id);
    }

    [Fact]
    public async Task Handle_AddStock_IncreasesQuantity()
    {
        var (context, productId) = await SeedData();
        var handler = new AdjustStockCommandHandler(context);

        var result = await handler.Handle(
            new AdjustStockCommand(productId, 5, "new stock", true), default);

        Assert.Equal(15, result.QuantityOnHand);
    }

    [Fact]
    public async Task Handle_RemoveStock_DecreasesQuantity()
    {
        var (context, productId) = await SeedData();
        var handler = new AdjustStockCommandHandler(context);

        var result = await handler.Handle(
            new AdjustStockCommand(productId, 3, "sold", false), default);

        Assert.Equal(7, result.QuantityOnHand);
    }

    [Fact]
    public async Task Handle_RemoveExcessiveStock_Throws()
    {
        var (context, productId) = await SeedData();
        var handler = new AdjustStockCommandHandler(context);

        await Assert.ThrowsAsync<InsufficientStockException>(() =>
            handler.Handle(new AdjustStockCommand(productId, 100, "too much", false), default));
    }

    [Fact]
    public async Task Handle_InvalidProductId_Throws()
    {
        var (context, _) = await SeedData();
        var handler = new AdjustStockCommandHandler(context);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(new AdjustStockCommand(Guid.NewGuid(), 1, "test", true), default));
    }
}
