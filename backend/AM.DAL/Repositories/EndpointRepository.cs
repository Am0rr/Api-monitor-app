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
                                                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is messing");

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    
    public async Task AddAsync(Endpoint endpoint)
    {
        const string sql = @"
            INSERT INTO Endpoints (Id, Name, Url, CheckIntervalSeconds, IsActive, CreatedAt)
            VALUES (@Id, @Name, @Url, @CheckIntervalSeconds, @IsActive, @CreatedAt);";

        using var connection = CreateConnection();
        await connection.ExecuteAsync(sql, endpoint);
    }

    public async Task UpdateAsync(Endpoint endpoint)
    {
        const string sql = @"
            UPDATE Endpoints
            SET Name = @Name,
                Url = @Url,
                CheckIntervalSeconds = @CheckIntervalSeconds,
                IsActive = @IsActive
            WHERE Id = @Id;";

        using var connection = CreateConnection();
        await connection.ExecuteAsync(sql, endpoint);
    }

    public async Task DeleteAsync(Guid endpointId)
    {
        const string sql = @"
                DELETE FROM Endpoints
                WHERE Id = @Id;";
        
        using var connection = CreateConnection();
        await connection.ExecuteAsync(sql, new {Id = endpointId});
    }

    public async Task<Endpoint?> GetByIdAsync(Guid endpointId)
    {
        const string sql = @"
                SELECT Id, Name, Url, CheckIntervalSeconds, IsActive, CreatedAt
                FROM Endpoints
                WHERE Id = @Id;";
        
        using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Endpoint>(sql, new {Id = endpointId});
    }

    public async Task<IEnumerable<Endpoint>> GetAllAsync()
    {
        const string sql = @"
                SELECT Id, Name, Url, CheckIntervalSeconds, IsActive, CreatedAt
                FROM Endpoints
                ORDER BY CreatedAt DESC;";

        using var connection = CreateConnection();
        return await connection.QueryAsync<Endpoint>(sql);
    }

    public async Task<IEnumerable<Endpoint>> GetActiveAsync()
    {
        const string sql = @"
                SELECT Id, Name, Url, CheckIntervalSeconds, IsActive, CreatedAt
                FROM Endpoints
                WHERE IsActive = 1
                ORDER BY CreatedAt DESC ;";
        
        using var connection = CreateConnection();
        return await connection.QueryAsync<Endpoint>(sql);
    }
}