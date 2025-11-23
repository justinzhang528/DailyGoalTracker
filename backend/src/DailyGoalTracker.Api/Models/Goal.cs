namespace DailyGoalTracker.Api.Models;

public class Goal
{
    public int Id { get; set; }
    public int TeamMemberId { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsComplete { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

