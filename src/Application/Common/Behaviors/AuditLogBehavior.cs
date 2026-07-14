using System.Reflection;
using InventoryManagement.Application.Common.Interfaces;
using MediatR;

namespace InventoryManagement.Application.Common.Behaviors;

public class AuditLogBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IAuditLogService _auditLogService;

    public AuditLogBehavior(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        var requestType = request.GetType();
        if (requestType.Name.EndsWith("Command"))
        {
            var entityType = requestType.Name.Replace("Command", "");
            var entityId = requestType.GetProperty("Id")?.GetValue(request)?.ToString()
                ?? requestType.GetProperty("ProductId")?.GetValue(request)?.ToString()
                ?? "unknown";

            await _auditLogService.LogAsync(
                requestType.Name,
                entityType,
                entityId,
                details: System.Text.Json.JsonSerializer.Serialize(request),
                cancellationToken: cancellationToken
            );
        }

        return response;
    }
}
