using AM.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using AM.DAL.Entities;

namespace AM.DAL.Repositories;

public class ExecutionLogRepository(IConfiguration configuration) : IExecutionLogRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
                                                ?? throw new InvalidOperationException(
                                                    "Connection string 'DefaultConnection' is messing");

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task AddAsync(ExecutionLog log)
    {
        const string sql = @"
                INSERT INTO ExecutionLogs (EndpointId, StatusCode, ResponseTimeMs, IsSuccess, ErrorMessage, CheckedAt)
                VALUES (@EndpointId, @StatusCode, @ResponseTimeMs, @IsSuccess, @ErrorMessage, @CheckedAt);";

        using var connection = CreateConnection();
        await connection.ExecuteAsync(sql, log);
    }

    public async Task<ExecutionLog?> GetByIdAsync(long executionLogId)
    {
        const string sql = @"
                SELECT Id, EndpointId, StatusCode, ResponseTimeMs, IsSuccess, ErrorMessage, CheckedAt
                FROM ExecutionLogs
                WHERE Id = @Id;";

        using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<ExecutionLog>(sql, new { Id = executionLogId });
    }

    public async Task<IEnumerable<ExecutionLog>> GetByEndpointIdAsync(Guid endpointId, int count = 100)
    {
        const string sql = @"
            SELECT Top(@Count) Id, EndpointId, StatusCode, ResponseTimeMs, IsSuccess, ErrorMessage, CheckedAt
            FROM ExecutionLogs l
            WHERE EndpointId = @EndpointId
            ORDER BY CheckedAt DESC;";

        using var connection = CreateConnection();
        return await connection.QueryAsync<ExecutionLog>(sql, new {EndpointId = endpointId, Count = count});
    }
}