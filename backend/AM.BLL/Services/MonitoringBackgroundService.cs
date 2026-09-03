using System.Diagnostics;
using AM.BLL.DTOs.ExecutionLogs;
using AM.BLL.Mapping;
using AM.DAL.Entities;
using AM.DAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AM.BLL.Services;

public class MonitoringBackgroundService(
    IServiceScopeFactory scopeFactory,
    IHttpClientFactory clientFactory,
    ILogger<MonitoringBackgroundService> logger) 
    : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("MonitoringBackgroundService started.");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await PollActivateEndpointAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured during monitoring cycle initialization");
            }

            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
        }
        
        logger.LogInformation("MonitoringBackgroundService stopped");
    }

    private async Task PollActivateEndpointAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var endpointRepository = scope.ServiceProvider.GetRequiredService<IEndpointRepository>();
        var executionLogRepository = scope.ServiceProvider.GetRequiredService<IExecutionLogRepository>();

        var activeEndpoints = (await endpointRepository.GetActiveAsync()).ToList();

        if (activeEndpoints.Count == 0) return;

        var tasks = activeEndpoints.Select(endpoint =>
            CheckEndpointAnLogAsync(endpoint, executionLogRepository, cancellationToken));

        await Task.WhenAll(tasks);
    }

    private async Task CheckEndpointAnLogAsync(
        Endpoint endpoint, 
        IExecutionLogRepository executionLogRepository,
        CancellationToken cancellationToken)
    {
        var client = clientFactory.CreateClient();
        client.Timeout = TimeSpan.FromSeconds(5);

        var stopwatch = Stopwatch.StartNew();
        int? statusCode = null;
        bool isSuccess = false;
        string? errorMessage = null;

        try
        {
            using var response = await client.GetAsync(endpoint.Url, cancellationToken);
            stopwatch.Stop();

            statusCode = (int)response.StatusCode;
            isSuccess = response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            stopwatch.Stop();
            errorMessage = $"HTTP error: {ex.Message}";
        }
        catch (TaskCanceledException)
        {
            stopwatch.Stop();
            errorMessage = "Request timed out after 5 seconds";
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            errorMessage = $"Unexpected error: {ex.Message}";
        }

        var logRequest = new CreateExecutionLogRequest(
            endpoint.Id,
            statusCode,
            (int)stopwatch.ElapsedMilliseconds,
            isSuccess,
            errorMessage);

        await executionLogRepository.AddAsync(logRequest.ToEntity());
        
        logger.LogInformation(
            "Checked {Url} | Status {Status} | Success: {Success} | Time: {Time}ms",
            endpoint.Url, statusCode?.ToString() ?? "N/A", isSuccess, stopwatch.ElapsedMilliseconds);
    }
}