namespace AM.BLL.DTOs.ExecutionLogs;

public record CreateExecutionLogRequest(
    Guid EndpointId,
    int? StatusCode,
    int ResponseTimeMs,
    bool IsSuccess,
    string? ErrorMessage
);