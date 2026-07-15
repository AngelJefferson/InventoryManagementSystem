namespace InventoryManagement.Domain.Entities;

public class Employee
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; } = null!;
    public string Department { get; private set; } = string.Empty;
    public string Position { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Employee() { }

    public Employee(string fullName, string department, string position, string email, string phone)
    {
        Id = Guid.NewGuid();
        SetFullName(fullName);
        SetDepartment(department);
        SetPosition(position);
        SetEmail(email);
        SetPhone(phone);
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

    public void SetPosition(string position)
    {
        Position = position ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetEmail(string email)
    {
        Email = email ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetPhone(string phone)
    {
        Phone = phone ?? string.Empty;
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
