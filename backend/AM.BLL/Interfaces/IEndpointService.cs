using AM.BLL.DTOs.Endpoints;

namespace AM.BLL.Interfaces;

public interface IEndpointService
{
    Task<EndpointResponse> CreateAsync(CreateEndpointRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid endpointId, UpdateEndpointRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid endpointId, CancellationToken cancellationToken = default);
    Task<EndpointResponse> GetByIdAsync(Guid endpointId, CancellationToken cancellationToken = default);
    Task<IEnumerable<EndpointResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<EndpointResponse>> GetActiveAsync(CancellationToken cancellationToken = default);
}