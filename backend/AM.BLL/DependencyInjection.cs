using AM.BLL.Interfaces;
using AM.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AM.BLL;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        services.AddScoped<IEndpointService, EndpointService>();
        services.AddScoped<IExecutionLogService, ExecutionLogService>();

        services.AddHostedService<MonitoringBackgroundService>();

        return services;
    }
}