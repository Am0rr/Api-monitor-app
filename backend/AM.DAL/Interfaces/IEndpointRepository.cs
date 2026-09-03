using AM.DAL.Entities;

namespace AM.DAL.Interfaces;

public interface IEndpointRepository
{
    Task AddAsync(Endpoint endpoint, CancellationToken cancellationToken = default);
    Task UpdateAsync(Endpoint endpoint, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid endpointId, CancellationToken cancellationToken = default);
    Task<Endpoint> GetByIdAsync(Guid endpointId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Endpoint>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Endpoint>> GetActiveAsync(CancellationToken cancellationToken = default);
}