using AM.BLL.DTOs.ExecutionLogs;
using AM.DAL.Entities;

namespace AM.BLL.Interfaces;

public interface IExecutionLogService
{
    Task<ExecutionLogResponse> CreateAsync(CreateExecutionLogRequest request, CancellationToken cancellationToken = default);
    Task<ExecutionLogResponse> GetByIdAsync(long executionLogId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExecutionLogResponse>> GetByEndpointIdAsync(Guid endpointId, CancellationToken cancellationToken = default,
        int count = 100);
}