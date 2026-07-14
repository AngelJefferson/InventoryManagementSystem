using InventoryManagement.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace InventoryManagement.Infrastructure.Services;

public class MongoAuditLogService : IAuditLogService
{
    private readonly IMongoCollection<AuditLogEntry> _collection;

    public MongoAuditLogService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDb") ?? "mongodb://localhost:27017";
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("InventoryAudit");
        _collection = database.GetCollection<AuditLogEntry>("audit_logs");
    }

    public async Task LogAsync(string action, string entityType, string entityId, string? performedBy = null, string? details = null, CancellationToken cancellationToken = default)
    {
        var entry = new AuditLogEntry
        {
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            PerformedBy = performedBy,
            Details = details,
            Timestamp = DateTime.UtcNow
        };

        await _collection.InsertOneAsync(entry, cancellationToken: cancellationToken);
    }
}

public class AuditLogEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string? PerformedBy { get; set; }
    public string? Details { get; set; }
    public DateTime Timestamp { get; set; }
}
