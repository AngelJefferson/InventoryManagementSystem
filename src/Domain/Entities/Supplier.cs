namespace InventoryManagement.Domain.Entities;

public class Supplier
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string ContactName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public ICollection<Product> Products { get; private set; } = [];

    private Supplier() { }

    public Supplier(string name, string contactName, string email, string phone)
    {
        Id = Guid.NewGuid();
        SetName(name);
        SetContactName(contactName);
        SetEmail(email);
        SetPhone(phone);
        CreatedAt = DateTime.UtcNow;
    }

    public void SetName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetContactName(string contactName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(contactName);
        ContactName = contactName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetEmail(string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        Email = email;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetPhone(string phone)
    {
        Phone = phone ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;
    }
}
