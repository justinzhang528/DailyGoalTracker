namespace DailyGoalTracker.Api.Models;

public class ActivityRecord
{
    public int Id { get; set; }
    public string ActivityType { get; set; } = string.Empty;
    public int TeamMemberId { get; set; }
    public string TeamMemberName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? GoalId { get; set; }
    public string? GoalDescription { get; set; }
    public DateTime Timestamp { get; set; }
}

public enum ActivityType
{
    AddGoal,
    UpdateMood,
    CompleteGoal,
    DeleteGoal
}

