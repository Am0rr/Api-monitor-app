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

    public async Task AddAsync(ExecutionLog log, CancellationToken cancellationToken)
    {
        const string sql = @"
                INSERT INTO ExecutionLogs (EndpointId, StatusCode, ResponseTimeMs, IsSuccess, ErrorMessage, CheckedAt)
                VALUES (@EndpointId, @StatusCode, @ResponseTimeMs, @IsSuccess, @ErrorMessage, @CheckedAt);";

        using var connection = CreateConnection();
        var command = new CommandDefinition(sql, log, cancellationToken: cancellationToken);
        await connection.ExecuteAsync(command);
    }

    public async Task<ExecutionLog> GetByIdAsync(long executionLogId, CancellationToken cancellationToken)
    {
        const string sql = @"
                SELECT Id, EndpointId, StatusCode, ResponseTimeMs, IsSuccess, ErrorMessage, CheckedAt
                FROM ExecutionLogs
                WHERE Id = @Id;";

        using var connection = CreateConnection();
        var command = new CommandDefinition(sql, new { Id = executionLogId }, cancellationToken: cancellationToken);
        return await connection.QuerySingleAsync(command);
    }

    public async Task<IEnumerable<ExecutionLog>> GetByEndpointIdAsync(Guid endpointId, CancellationToken cancellationToken, int count = 100)
    {
        const string sql = @"
            SELECT Top(@Count) Id, EndpointId, StatusCode, ResponseTimeMs, IsSuccess, ErrorMessage, CheckedAt
            FROM ExecutionLogs l
            WHERE EndpointId = @EndpointId
            ORDER BY CheckedAt DESC;";

        using var connection = CreateConnection();
        var command = new CommandDefinition(sql, new {EndpointId = endpointId, Count = count}, cancellationToken: cancellationToken);
        return await connection.QueryAsync<ExecutionLog>(command);
    }
}