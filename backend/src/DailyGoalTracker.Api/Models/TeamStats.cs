namespace DailyGoalTracker.Api.Models;

public class TeamStats
{
    public int TotalGoals { get; set; }
    public int CompletedGoals { get; set; }
    public double CompletionPercentage { get; set; }
    public Dictionary<string, int> MoodCounts { get; set; } = new();
}

