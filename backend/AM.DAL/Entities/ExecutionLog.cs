namespace AM.DAL.Entities;

public class ExecutionLog
{
    public long Id { get; private set; }
    public DateTimeOffset CheckedAt { get; private set; }
    public Guid EndpointId { get; private set; }
    public int? StatusCode {get; private set; }
    public int ResponseTimeMs { get; private set; }
    public bool IsSuccess { get; private set; }
    public string? ErrorMessage { get; private set; }
    
    protected ExecutionLog() {}
    
    public ExecutionLog(Guid endpointId, int? statusCode, int responseTimeMs, bool isSuccess, string? errorMessage)
    {
        CheckedAt = DateTimeOffset.UtcNow;
        EndpointId = endpointId;
        StatusCode = statusCode;
        ResponseTimeMs = responseTimeMs;
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }
}
