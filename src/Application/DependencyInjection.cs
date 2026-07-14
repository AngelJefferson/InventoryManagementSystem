using FluentValidation;
using InventoryManagement.Application.Common.Behaviors;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuditLogBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
