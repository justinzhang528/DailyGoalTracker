using Microsoft.Data.Sqlite;
using DailyGoalTracker.Api.Configuration;

namespace DailyGoalTracker.Api.Data.Database;

public class DatabaseInitializer
{
    public static void Initialize()
    {
        var dbPath = DatabaseConfig.GetDatabasePath();
        var dbDirectory = Path.GetDirectoryName(dbPath);
        
        if (!string.IsNullOrEmpty(dbDirectory) && !Directory.Exists(dbDirectory))
        {
            Directory.CreateDirectory(dbDirectory);
        }

        using var connection = DatabaseConfig.CreateConnection();
        connection.Open();

        var schemaPath = Path.Combine(
            AppContext.BaseDirectory,
            "..", "..", "..", "..", "..", "..", "..",
            "Data", "Database", "schema.sql"
        );
        
        var schemaPathFull = Path.GetFullPath(schemaPath);
        
        if (File.Exists(schemaPathFull))
        {
            var schema = File.ReadAllText(schemaPathFull);
            using var command = new SqliteCommand(schema, connection);
            command.ExecuteNonQuery();
        }
        else
        {
            // Fallback: Create schema inline if file not found
            var schema = @"
                CREATE TABLE IF NOT EXISTS TeamMembers (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    CurrentMood TEXT CHECK(CurrentMood IN ('😀', '😊', '😐', '😞', '😤') OR CurrentMood IS NULL)
                );
                CREATE TABLE IF NOT EXISTS Goals (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    TeamMemberId INTEGER NOT NULL,
                    Description TEXT NOT NULL CHECK(LENGTH(Description) > 0 AND LENGTH(Description) <= 500),
                    IsComplete INTEGER NOT NULL DEFAULT 0 CHECK(IsComplete IN (0, 1)),
                    CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
                    UpdatedAt TEXT NOT NULL DEFAULT (datetime('now')),
                    FOREIGN KEY (TeamMemberId) REFERENCES TeamMembers(Id) ON DELETE CASCADE
                );
                CREATE INDEX IF NOT EXISTS idx_goals_teammemberid ON Goals(TeamMemberId);
                CREATE INDEX IF NOT EXISTS idx_goals_iscomplete ON Goals(IsComplete);
                CREATE TABLE IF NOT EXISTS ActivityRecords (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ActivityType TEXT NOT NULL CHECK(ActivityType IN ('AddGoal', 'UpdateMood', 'CompleteGoal', 'DeleteGoal')),
                    TeamMemberId INTEGER NOT NULL,
                    TeamMemberName TEXT NOT NULL CHECK(LENGTH(TeamMemberName) > 0),
                    Description TEXT NOT NULL CHECK(LENGTH(Description) > 0),
                    GoalId INTEGER NULL,
                    GoalDescription TEXT NULL,
                    Timestamp TEXT NOT NULL DEFAULT (datetime('now')),
                    FOREIGN KEY (TeamMemberId) REFERENCES TeamMembers(Id),
                    FOREIGN KEY (GoalId) REFERENCES Goals(Id) ON DELETE SET NULL
                );
                CREATE INDEX IF NOT EXISTS idx_activityrecords_timestamp ON ActivityRecords(Timestamp DESC);
                CREATE INDEX IF NOT EXISTS idx_activityrecords_teammemberid ON ActivityRecords(TeamMemberId);
                CREATE INDEX IF NOT EXISTS idx_activityrecords_activitytype ON ActivityRecords(ActivityType);
            ";
            using var command = new SqliteCommand(schema, connection);
            command.ExecuteNonQuery();
        }
    }
}

