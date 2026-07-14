namespace InventoryManagement.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public ICollection<Product> Products { get; private set; } = [];

    private Category() { }

    public Category(string name, string description)
    {
        Id = Guid.NewGuid();
        SetName(name);
        SetDescription(description);
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
}
