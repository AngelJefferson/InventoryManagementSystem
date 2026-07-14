namespace InventoryManagement.Application.Common.Interfaces;

public interface IAuditLogService
{
    Task LogAsync(string action, string entityType, string entityId, string? performedBy = null, string? details = null, CancellationToken cancellationToken = default);
}
