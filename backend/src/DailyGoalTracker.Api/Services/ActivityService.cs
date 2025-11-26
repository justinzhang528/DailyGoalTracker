using DailyGoalTracker.Api.Data.Dapper;
using DailyGoalTracker.Api.Models;

namespace DailyGoalTracker.Api.Services;

public class ActivityService
{
    private readonly ActivityRepository _repository;

    public ActivityService(ActivityRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ActivityRecord>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task RecordActivityAsync(ActivityType activityType, int teamMemberId, string teamMemberName, string description, int? goalId = null, string? goalDescription = null)
    {
        var activityRecord = new ActivityRecord
        {
            ActivityType = activityType.ToString(),
            TeamMemberId = teamMemberId,
            TeamMemberName = teamMemberName,
            Description = description,
            GoalId = goalId,
            GoalDescription = goalDescription,
            Timestamp = DateTime.UtcNow
        };

        await _repository.CreateAsync(activityRecord);
    }
}

