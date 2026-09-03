namespace AM.BLL.DTOs.ExecutionLogs;

public record ExecutionLogResponse(
    long Id,
    DateTimeOffset CheckedAt,
    Guid EndpointId,
    int? StatusCode,
    int ResponseTimeMs,
    bool IsSuccess,
    string? ErrorMessage
);