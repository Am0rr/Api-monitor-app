namespace AM.BLL.DTOs.Endpoints;

public record EndpointResponse(
    Guid Id,
    DateTimeOffset CreatedAt,
    string Name,
    string Url,
    int CheckIntervalSeconds,
    bool IsActive
);