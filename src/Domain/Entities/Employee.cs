namespace InventoryManagement.Domain.Entities;

public class Employee
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; } = null!;
    public string Department { get; private set; } = string.Empty;
    public string Sede { get; private set; } = string.Empty;
    public string Position { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public ICollection<Product> Products { get; private set; } = new List<Product>();

    private Employee() { }

    public Employee(string fullName, string department, string sede, string position)
    {
        Id = Guid.NewGuid();
        SetFullName(fullName);
        SetDepartment(department);
        SetSede(sede);
        SetPosition(position);
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void SetFullName(string fullName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);
        FullName = fullName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetDepartment(string department)
    {
        Department = department ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetSede(string sede)
    {
        Sede = sede ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetPosition(string position)
    {
        Position = position ?? string.Empty;
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
