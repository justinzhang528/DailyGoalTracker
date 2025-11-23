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
        
        // Find team-members.json by searching up from the current directory
        // It should be at backend/team-members.json relative to the solution root
        var currentDir = Directory.GetCurrentDirectory();
        var searchDir = currentDir;
        
        for (int i = 0; i < 10; i++)
        {
            // Check if we're in the backend directory or can find it
            var backendPath = Path.Combine(searchDir, "team-members.json");
            if (File.Exists(backendPath))
            {
                _configPath = Path.GetFullPath(backendPath);
                _logger.LogInformation("Found team-members.json at: {Path}", _configPath);
                return;
            }
            
            // Also check in backend/ subdirectory
            var backendSubPath = Path.Combine(searchDir, "backend", "team-members.json");
            if (File.Exists(backendSubPath))
            {
                _configPath = Path.GetFullPath(backendSubPath);
                _logger.LogInformation("Found team-members.json at: {Path}", _configPath);
                return;
            }
            
            var parent = Directory.GetParent(searchDir);
            if (parent == null) break;
            searchDir = parent.FullName;
        }
        
        // Fallback: try relative to AppContext.BaseDirectory (bin/Debug/net8.0/)
        var baseDir = AppContext.BaseDirectory;
        _configPath = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "..", "team-members.json"));
        _logger.LogWarning("Using fallback path for team-members.json: {Path}", _configPath);
    }

    public async Task LoadTeamMembersAsync()
    {
        if (!File.Exists(_configPath))
        {
            _logger.LogWarning("team-members.json not found at {Path}", _configPath);
            return;
        }

        var json = await File.ReadAllTextAsync(_configPath);
        _logger.LogInformation("Reading team-members.json content: {Content}", json);
        
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var configMembers = JsonSerializer.Deserialize<List<TeamMemberConfig>>(json, options);

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
        [System.Text.Json.Serialization.JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}

