using DailyGoalTracker.Api.Data.Dapper;
using DailyGoalTracker.Api.Models;
using Dapper;

namespace DailyGoalTracker.Api.Data.Dapper;

public class TeamMemberRepository : BaseRepository
{
    public async Task<IEnumerable<TeamMember>> GetAllAsync()
    {
        const string sql = @"
            SELECT Id, Name, CurrentMood
            FROM TeamMembers
            ORDER BY Name";
        
        return await QueryAsync<TeamMember>(sql);
    }

    public async Task<TeamMember?> GetByIdAsync(int id)
    {
        const string sql = @"
            SELECT Id, Name, CurrentMood
            FROM TeamMembers
            WHERE Id = @Id";
        
        return await QueryFirstOrDefaultAsync<TeamMember>(sql, new { Id = id });
    }

    public async Task UpdateMoodAsync(int id, string? mood)
    {
        const string sql = @"
            UPDATE TeamMembers
            SET CurrentMood = @Mood
            WHERE Id = @Id";
        
        await ExecuteAsync(sql, new { Id = id, Mood = mood });
    }
}

