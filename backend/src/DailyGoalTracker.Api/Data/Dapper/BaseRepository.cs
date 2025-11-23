using Microsoft.Data.Sqlite;
using DailyGoalTracker.Api.Configuration;
using Dapper;

namespace DailyGoalTracker.Api.Data.Dapper;

public abstract class BaseRepository
{
    protected SqliteConnection GetConnection()
    {
        return DatabaseConfig.CreateConnection();
    }

    protected async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null) where T : class
    {
        using var connection = GetConnection();
        connection.Open();
        return await connection.QueryFirstOrDefaultAsync<T>(sql, param);
    }

    protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
    {
        using var connection = GetConnection();
        connection.Open();
        return await connection.QueryAsync<T>(sql, param);
    }

    protected async Task<int> ExecuteAsync(string sql, object? param = null)
    {
        using var connection = GetConnection();
        connection.Open();
        return await connection.ExecuteAsync(sql, param);
    }

    protected async Task<T?> ExecuteScalarAsync<T>(string sql, object? param = null) where T : struct
    {
        using var connection = GetConnection();
        connection.Open();
        var result = await connection.ExecuteScalarAsync<T?>(sql, param);
        return result ?? default(T?);
    }
}

