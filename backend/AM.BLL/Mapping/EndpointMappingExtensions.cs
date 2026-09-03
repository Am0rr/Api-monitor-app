using AM.BLL.DTOs.Endpoints;
using AM.DAL.Entities;

namespace AM.BLL.Mapping;

public static class EndpointMappingExtensions
{
    public static Endpoint ToEntity(this CreateEndpointRequest request)
    {
        return new Endpoint(
            request.Name,
            request.Url,
            request.CheckIntervalSeconds,
            request.IsActive);
    }

    public static EndpointResponse ToResponse(this Endpoint endpoint)
    {
        return new EndpointResponse(
            endpoint.Id,
            endpoint.CreatedAt,
            endpoint.Name,
            endpoint.Url,
            endpoint.CheckIntervalSeconds,
            endpoint.IsActive);
    }
}