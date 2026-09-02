using AM.DAL.Entities;

namespace AM.DAL.Interfaces;

public interface IExecutionLogRepository
{
    Task AddAsync(ExecutionLog log);
    Task<ExecutionLog?> GetByIdAsync(Guid executionLogId);
    Task<IEnumerable<ExecutionLog>> GetByEndpointIdAsync(Guid endpointId, int count = 100);
}