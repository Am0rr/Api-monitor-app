using System.Data;
using AM.DAL.Entities;
using AM.DAL.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AM.DAL.Repositories;

public class EndpointRepository(IConfiguration configuration) : IEndpointRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
                                                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing");

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    
    public async Task AddAsync(Endpoint endpoint, CancellationToken cancellationToken)
    {
        const string sql = @"
            INSERT INTO Endpoints (Id, Name, Url, CheckIntervalSeconds, IsActive, CreatedAt)
            VALUES (@Id, @Name, @Url, @CheckIntervalSeconds, @IsActive, @CreatedAt);";

        using var connection = CreateConnection();
        var command = new CommandDefinition(sql, endpoint, cancellationToken: cancellationToken);
        await connection.ExecuteAsync(command);
    }

    public async Task UpdateAsync(Endpoint endpoint, CancellationToken cancellationToken)
    {
        const string sql = @"
            UPDATE Endpoints
            SET Name = @Name,
                Url = @Url,
                CheckIntervalSeconds = @CheckIntervalSeconds,
                IsActive = @IsActive
            WHERE Id = @Id;";

        using var connection = CreateConnection();
        var command = new CommandDefinition(sql, endpoint, cancellationToken: cancellationToken);
        await connection.ExecuteAsync(command);
    }

    public async Task DeleteAsync(Guid endpointId, CancellationToken cancellationToken)
    {
        const string sql = @"
                DELETE FROM Endpoints
                WHERE Id = @Id;";
        
        using var connection = CreateConnection();
        var command = new CommandDefinition(sql, new {Id = endpointId}, cancellationToken: cancellationToken);
        await connection.ExecuteAsync(command);
    }

    public async Task<Endpoint?> GetByIdAsync(Guid endpointId, CancellationToken cancellationToken)
    {
        const string sql = @"
                SELECT Id, Name, Url, CheckIntervalSeconds, IsActive, CreatedAt
                FROM Endpoints
                WHERE Id = @Id;";
        
        using var connection = CreateConnection();
        var command = new CommandDefinition(sql, new {Id = endpointId}, cancellationToken: cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<Endpoint>(command);
    }

    public async Task<IEnumerable<Endpoint>> GetAllAsync(CancellationToken cancellationToken)
    {
        const string sql = @"
                SELECT Id, Name, Url, CheckIntervalSeconds, IsActive, CreatedAt
                FROM Endpoints
                ORDER BY CreatedAt DESC;";

        using var connection = CreateConnection();
        var command = new CommandDefinition(sql, cancellationToken: cancellationToken);
        return await connection.QueryAsync<Endpoint>(command);
    }

    public async Task<IEnumerable<Endpoint>> GetActiveAsync(CancellationToken cancellationToken)
    {
        const string sql = @"
                SELECT Id, Name, Url, CheckIntervalSeconds, IsActive, CreatedAt
                FROM Endpoints
                WHERE IsActive = 1
                ORDER BY CreatedAt DESC ;";
        
        using var connection = CreateConnection();
        var command = new CommandDefinition(sql, cancellationToken: cancellationToken);
        return await connection.QueryAsync<Endpoint>(command);
    }
}