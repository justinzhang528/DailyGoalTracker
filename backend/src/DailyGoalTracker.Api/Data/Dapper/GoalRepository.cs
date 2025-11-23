using DailyGoalTracker.Api.Data.Dapper;
using DailyGoalTracker.Api.Models;
using Dapper;

namespace DailyGoalTracker.Api.Data.Dapper;

public class GoalRepository : BaseRepository
{
    public async Task<IEnumerable<Goal>> GetAllAsync(int? teamMemberId = null)
    {
        string sql = @"
            SELECT Id, TeamMemberId, Description, IsComplete, 
                   datetime(CreatedAt) as CreatedAt, 
                   datetime(UpdatedAt) as UpdatedAt
            FROM Goals";
        
        if (teamMemberId.HasValue)
        {
            sql += " WHERE TeamMemberId = @TeamMemberId";
        }
        
        sql += " ORDER BY CreatedAt DESC";
        
        return await QueryAsync<Goal>(sql, teamMemberId.HasValue ? new { TeamMemberId = teamMemberId.Value } : null);
    }

    public async Task<Goal?> GetByIdAsync(int id)
    {
        const string sql = @"
            SELECT Id, TeamMemberId, Description, IsComplete,
                   datetime(CreatedAt) as CreatedAt,
                   datetime(UpdatedAt) as UpdatedAt
            FROM Goals
            WHERE Id = @Id";
        
        return await QueryFirstOrDefaultAsync<Goal>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(Goal goal)
    {
        const string sql = @"
            INSERT INTO Goals (TeamMemberId, Description, IsComplete, CreatedAt, UpdatedAt)
            VALUES (@TeamMemberId, @Description, @IsComplete, datetime('now'), datetime('now'));
            SELECT last_insert_rowid();";
        
        var id = await ExecuteScalarAsync<long>(sql, new
        {
            TeamMemberId = goal.TeamMemberId,
            Description = goal.Description,
            IsComplete = goal.IsComplete ? 1 : 0
        });
        
        return (int)id;
    }

    public async Task UpdateAsync(Goal goal)
    {
        const string sql = @"
            UPDATE Goals
            SET Description = @Description,
                IsComplete = @IsComplete,
                UpdatedAt = datetime('now')
            WHERE Id = @Id";
        
        await ExecuteAsync(sql, new
        {
            Id = goal.Id,
            Description = goal.Description,
            IsComplete = goal.IsComplete ? 1 : 0
        });
    }

    public async Task DeleteAsync(int id)
    {
        const string sql = "DELETE FROM Goals WHERE Id = @Id";
        await ExecuteAsync(sql, new { Id = id });
    }
}

