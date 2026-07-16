namespace InventoryManagement.Application.DTOs;

public class EmployeeDto
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Department { get; init; } = string.Empty;
    public string Position { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public int AssignedEquipmentCount { get; init; }
    public DateTime CreatedAt { get; init; }
}
