using AM.BLL.DTOs.ExecutionLogs;
using AM.DAL.Entities;

namespace AM.BLL.Mapping;

public static class ExecutionLogMappingExtensions
{
    public static ExecutionLog ToEntity(this CreateExecutionLogRequest request)
    {
        return new ExecutionLog(
            request.EndpointId,
            request.StatusCode,
            request.ResponseTimeMs,
            request.IsSuccess,
            request.ErrorMessage);
    }

    public static ExecutionLogResponse ToResponse(this ExecutionLog log)
    {
        return new ExecutionLogResponse(
            log.Id,
            log.CheckedAt,
            log.EndpointId,
            log.StatusCode,
            log.ResponseTimeMs,
            log.IsSuccess,
            log.ErrorMessage);
    }
}