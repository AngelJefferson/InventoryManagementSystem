using InventoryManagement.Domain.ValueObjects;

namespace InventoryManagement.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string SKU { get; private set; } = null!;
    public Money Price { get; private set; } = null!;
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;
    public Guid? SupplierId { get; private set; }
    public Supplier? Supplier { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Product() { }

    public Product(string name, string description, string sku, Money price, Guid categoryId, Guid? supplierId = null)
    {
        Id = Guid.NewGuid();
        SetName(name);
        SetDescription(description);
        SetSKU(sku);
        SetPrice(price);
        CategoryId = categoryId;
        SupplierId = supplierId;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void SetName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetDescription(string description)
    {
        Description = description ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetSKU(string sku)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);
        SKU = sku.ToUpperInvariant();
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetPrice(Money price)
    {
        Price = price ?? throw new ArgumentNullException(nameof(price));
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignCategory(Guid categoryId)
    {
        CategoryId = categoryId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignSupplier(Guid? supplierId)
    {
        SupplierId = supplierId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
