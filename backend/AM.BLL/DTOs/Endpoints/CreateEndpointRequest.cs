namespace AM.BLL.DTOs.Endpoints;

public record CreateEndpointRequest(
    string Name,
    string Url, 
    int CheckIntervalSeconds,
    bool IsActive
);
