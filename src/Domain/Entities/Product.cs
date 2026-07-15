namespace InventoryManagement.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string SKU { get; private set; } = null!;
    public string Model { get; private set; } = string.Empty;
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;
    public Guid? SupplierId { get; private set; }
    public Supplier? Supplier { get; private set; }
    public Guid? EmployeeId { get; private set; }
    public Employee? Employee { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Product() { }

    public Product(string name, string description, string sku, Guid categoryId, Guid? supplierId = null, string model = "", Guid? employeeId = null)
    {
        Id = Guid.NewGuid();
        SetName(name);
        SetDescription(description);
        SetSKU(sku);
        SetModel(model);
        CategoryId = categoryId;
        SupplierId = supplierId;
        EmployeeId = employeeId;
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

    public void SetModel(string model)
    {
        Model = model ?? string.Empty;
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

    public void AssignEmployee(Guid? employeeId)
    {
        EmployeeId = employeeId;
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
