using Microsoft.Data.Sqlite;

namespace DailyGoalTracker.Api.Configuration;

public static class DatabaseConfig
{
    public static string GetDatabasePath()
    {
        var baseDirectory = AppContext.BaseDirectory;
        var dbPath = Path.Combine(baseDirectory, "..", "..", "..", "..", "..", "dailygoals.db");
        return Path.GetFullPath(dbPath);
    }

    public static string ConnectionString => $"Data Source={GetDatabasePath()};";

    public static SqliteConnection CreateConnection()
    {
        return new SqliteConnection(ConnectionString);
    }
}

