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
    public Guid? EmployeeId { get; private set; }
    public Employee? Employee { get; private set; }
    public string? AssetNumber { get; private set; }
    public string Department { get; private set; } = string.Empty;
    public string PhysicalLocation { get; private set; } = string.Empty;
    public string OperatingSystem { get; private set; } = string.Empty;
    public string HardwareConfiguration { get; private set; } = string.Empty;
    public string Status { get; private set; } = string.Empty;
    public DateTime? AcquisitionDate { get; private set; }
    public string Observations { get; private set; } = string.Empty;
    public DateTime? MaintenanceDate { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Product() { }

    public Product(string name, string description, string sku, Guid categoryId,
        string model = "", Guid? employeeId = null,
        string? assetNumber = null, string department = "", string physicalLocation = "",
        string operatingSystem = "", string hardwareConfiguration = "",
        string status = "", DateTime? acquisitionDate = null,
        string observations = "", DateTime? maintenanceDate = null)
    {
        Id = Guid.NewGuid();
        SetName(name);
        SetDescription(description);
        SetSKU(sku);
        SetModel(model);
        CategoryId = categoryId;
        EmployeeId = employeeId;
        AssetNumber = assetNumber;
        Department = department;
        PhysicalLocation = physicalLocation;
        OperatingSystem = operatingSystem;
        HardwareConfiguration = hardwareConfiguration;
        Status = status;
        AcquisitionDate = acquisitionDate;
        Observations = observations;
        MaintenanceDate = maintenanceDate;
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

    public void SetAssetNumber(string? assetNumber)
    {
        AssetNumber = assetNumber;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetDepartment(string department)
    {
        Department = department ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetPhysicalLocation(string location)
    {
        PhysicalLocation = location ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetOperatingSystem(string os)
    {
        OperatingSystem = os ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetHardwareConfiguration(string config)
    {
        HardwareConfiguration = config ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetStatus(string status)
    {
        Status = status ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetAcquisitionDate(DateTime? date)
    {
        AcquisitionDate = date;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetObservations(string observations)
    {
        Observations = observations ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetMaintenanceDate(DateTime? date)
    {
        MaintenanceDate = date;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignCategory(Guid categoryId)
    {
        CategoryId = categoryId;
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
