using AM.DAL.Interfaces;
using AM.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AM.DAL;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services)
    {
        services.AddScoped<IEndpointRepository, EndpointRepository>();
        services.AddScoped<IExecutionLogRepository, ExecutionLogRepository>();

        return services;
    }
}