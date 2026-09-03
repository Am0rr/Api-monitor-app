using AM.DAL.Entities;

namespace AM.DAL.Interfaces;

public interface IExecutionLogRepository
{
    Task AddAsync(ExecutionLog log, CancellationToken cancellationToken = default);
    Task<ExecutionLog?> GetByIdAsync(long executionLogId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExecutionLog>> GetByEndpointIdAsync(Guid endpointId, CancellationToken cancellationToken, int count = 100);
}