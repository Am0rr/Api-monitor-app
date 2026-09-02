using AM.DAL.Entities;

namespace AM.DAL.Interfaces;

public interface IEndpointRepository
{
    Task AddAsync(Endpoint endpoint);
    Task UpdateAsync(Endpoint endpoint);
    Task DeleteAsync(Guid endpointId);
    Task<Endpoint?> GetByIdAsync(Guid endpointId);
    Task<IEnumerable<Endpoint>> GetAllAsync();
    Task<IEnumerable<Endpoint>> GetActiveAsync();
}