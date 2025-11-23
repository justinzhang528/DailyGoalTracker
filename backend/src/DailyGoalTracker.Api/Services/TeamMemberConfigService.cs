using System.Text.Json;
using DailyGoalTracker.Api.Models;
using DailyGoalTracker.Api.Configuration;
using Microsoft.Data.Sqlite;

namespace DailyGoalTracker.Api.Services;

public class TeamMemberConfigService
{
    private readonly string _configPath;
    private readonly ILogger<TeamMemberConfigService> _logger;

    public TeamMemberConfigService(ILogger<TeamMemberConfigService> logger)
    {
        _logger = logger;
        var baseDirectory = AppContext.BaseDirectory;
        _configPath = Path.Combine(baseDirectory, "..", "..", "..", "..", "..", "..", "team-members.json");
        _configPath = Path.GetFullPath(_configPath);
    }

    public async Task LoadTeamMembersAsync()
    {
        if (!File.Exists(_configPath))
        {
            _logger.LogWarning("team-members.json not found at {Path}", _configPath);
            return;
        }

        var json = await File.ReadAllTextAsync(_configPath);
        var configMembers = JsonSerializer.Deserialize<List<TeamMemberConfig>>(json);

        if (configMembers == null || configMembers.Count == 0)
        {
            _logger.LogWarning("No team members found in config file");
            return;
        }

        using var connection = DatabaseConfig.CreateConnection();
        connection.Open();

        foreach (var configMember in configMembers)
        {
            // Check if team member already exists
            using var checkCmd = new SqliteCommand(
                "SELECT Id FROM TeamMembers WHERE Name = @Name",
                connection
            );
            checkCmd.Parameters.AddWithValue("@Name", configMember.Name);
            var existingId = checkCmd.ExecuteScalar();

            if (existingId == null)
            {
                // Insert new team member
                using var insertCmd = new SqliteCommand(
                    "INSERT INTO TeamMembers (Name) VALUES (@Name)",
                    connection
                );
                insertCmd.Parameters.AddWithValue("@Name", configMember.Name);
                insertCmd.ExecuteNonQuery();
                _logger.LogInformation("Added team member: {Name}", configMember.Name);
            }
        }
    }

    private class TeamMemberConfig
    {
        public string Name { get; set; } = string.Empty;
    }
}

