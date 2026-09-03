using AM.BLL.DTOs.Endpoints;
using AM.BLL.Exceptions;
using AM.BLL.Interfaces;
using AM.BLL.Mapping;
using AM.DAL.Entities;
using AM.DAL.Interfaces;

namespace AM.BLL.Services;

public class EndpointService(
    IEndpointRepository endpointRepository) 
    : IEndpointService
{
    public async Task<EndpointResponse> CreateAsync(CreateEndpointRequest request, CancellationToken cancellationToken)
    {
        var endpoint = request.ToEntity();

        await endpointRepository.AddAsync(endpoint, cancellationToken);
        
        return endpoint.ToResponse();
    }

    public async Task UpdateAsync(Guid endpointId, UpdateEndpointRequest request, CancellationToken cancellationToken)
    {
        var endpoint = await endpointRepository.GetByIdAsync(endpointId, cancellationToken)
                    ?? throw new NotFoundException($"Endpoint with ID {endpointId} was not found.");
        
        endpoint.Update(
            request.Name,
            request.Url,
            request.CheckIntervalSeconds,
            request.IsActive);

        await endpointRepository.UpdateAsync(endpoint, cancellationToken);
    }

    public async Task DeleteAsync(Guid endpointId, CancellationToken cancellationToken)
    {
        bool deleted = await endpointRepository.DeleteAsync(endpointId, cancellationToken);
        
        if(!deleted)
            throw new NotFoundException($"Endpoint with ID {endpointId} was not found.");
    }

    public async Task<EndpointResponse> GetByIdAsync(Guid endpointId, CancellationToken cancellationToken)
    {
        var endpoint = await endpointRepository.GetByIdAsync(endpointId, cancellationToken)
                       ?? throw new NotFoundException($"Endpoint with ID {endpointId} was not found.");

        return endpoint.ToResponse();
    }

    public async Task<IEnumerable<EndpointResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var endpoints = await endpointRepository.GetAllAsync(cancellationToken);

        return endpoints.Select(e => e.ToResponse());
    }

    public async Task<IEnumerable<EndpointResponse>> GetActiveAsync(CancellationToken cancellationToken)
    {
        var endpoints = await endpointRepository.GetActiveAsync(cancellationToken);

        return endpoints.Select(e => e.ToResponse());
    }
}