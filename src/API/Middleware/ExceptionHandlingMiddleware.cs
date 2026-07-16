using System.Net;
using System.Text.Json;
using FluentValidation;
using InventoryManagement.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var message = exception.Message;

        switch (exception)
        {
            case InsufficientStockException:
                code = HttpStatusCode.Conflict;
                break;
            case ValidationException:
                code = HttpStatusCode.BadRequest;
                break;
            case KeyNotFoundException:
                code = HttpStatusCode.NotFound;
                break;
            case InvalidOperationException:
                code = HttpStatusCode.BadRequest;
                break;
            case ArgumentException:
                code = HttpStatusCode.BadRequest;
                break;
            case DbUpdateException:
                code = HttpStatusCode.Conflict;
                message = "Error de base de datos: " + exception.InnerException?.Message ?? exception.Message;
                break;
            default:
                message = $"Error interno ({exception.GetType().Name}): {exception.Message}";
                break;
        }

        if (code == HttpStatusCode.InternalServerError)
            _logger.LogError(exception, "HTTP {Method} {Path} devolvió {StatusCode}: {Message}",
                context.Request.Method, context.Request.Path, (int)code, exception.Message);
        else
            _logger.LogWarning(exception, "HTTP {Method} {Path} devolvió {StatusCode}: {Message}",
                context.Request.Method, context.Request.Path, (int)code, exception.Message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        var result = JsonSerializer.Serialize(new { error = message });
        await context.Response.WriteAsync(result);
    }
}
