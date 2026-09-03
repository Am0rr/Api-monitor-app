namespace AM.BLL.DTOs.Endpoints;

public record UpdateEndpointRequest(
    string Name,
    string Url,
    int CheckIntervalSeconds,
    bool IsActive
);