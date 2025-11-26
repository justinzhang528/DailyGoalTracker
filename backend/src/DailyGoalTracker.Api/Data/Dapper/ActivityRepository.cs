using DailyGoalTracker.Api.Models;
using Dapper;

namespace DailyGoalTracker.Api.Data.Dapper;

public class ActivityRepository : BaseRepository
{
    public async Task<IEnumerable<ActivityRecord>> GetAllAsync()
    {
        const string sql = @"
            SELECT Id, ActivityType, TeamMemberId, TeamMemberName, Description, 
                   GoalId, GoalDescription, 
                   datetime(Timestamp) as Timestamp
            FROM ActivityRecords
            ORDER BY Timestamp DESC";
        
        return await QueryAsync<ActivityRecord>(sql);
    }

    public async Task<ActivityRecord?> GetByIdAsync(int id)
    {
        const string sql = @"
            SELECT Id, ActivityType, TeamMemberId, TeamMemberName, Description,
                   GoalId, GoalDescription,
                   datetime(Timestamp) as Timestamp
            FROM ActivityRecords
            WHERE Id = @Id";
        
        return await QueryFirstOrDefaultAsync<ActivityRecord>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(ActivityRecord activityRecord)
    {
        const string sql = @"
            INSERT INTO ActivityRecords (ActivityType, TeamMemberId, TeamMemberName, Description, GoalId, GoalDescription, Timestamp)
            VALUES (@ActivityType, @TeamMemberId, @TeamMemberName, @Description, @GoalId, @GoalDescription, datetime('now'));
            SELECT last_insert_rowid();";
        
        var id = await ExecuteScalarAsync<long>(sql, new
        {
            ActivityType = activityRecord.ActivityType,
            TeamMemberId = activityRecord.TeamMemberId,
            TeamMemberName = activityRecord.TeamMemberName,
            Description = activityRecord.Description,
            GoalId = activityRecord.GoalId,
            GoalDescription = activityRecord.GoalDescription
        });
        
        return (int)id;
    }
}

