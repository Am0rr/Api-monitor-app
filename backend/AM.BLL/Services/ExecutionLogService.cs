using AM.BLL.DTOs.ExecutionLogs;
using AM.BLL.Exceptions;
using AM.BLL.Interfaces;
using AM.BLL.Mapping;
using AM.DAL.Interfaces;

namespace AM.BLL.Services;

public class ExecutionLogService(
    IExecutionLogRepository logRepository) 
    : IExecutionLogService
{
    public async Task<ExecutionLogResponse> CreateAsync(CreateExecutionLogRequest request, CancellationToken cancellationToken)
    {
        var log = request.ToEntity();

        await logRepository.AddAsync(log, cancellationToken);

        return log.ToResponse();
    }

    public async Task<ExecutionLogResponse> GetByIdAsync(long executionLogId, CancellationToken cancellationToken)
    {
        var log = await logRepository.GetByIdAsync(executionLogId, cancellationToken)
                  ?? throw new NotFoundException($"Execution log with ID {executionLogId} was not found.");

        return log.ToResponse();
    }

    public async Task<IEnumerable<ExecutionLogResponse>> GetByEndpointIdAsync(Guid endpointId,
        CancellationToken cancellationToken, int count = 100)
    {
        var logs = await logRepository.GetByEndpointIdAsync(endpointId, cancellationToken, count);

        return logs.Select(e => e.ToResponse());
    }
}