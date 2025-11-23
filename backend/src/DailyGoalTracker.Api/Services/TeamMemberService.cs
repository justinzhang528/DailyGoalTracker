using DailyGoalTracker.Api.Data.Dapper;
using DailyGoalTracker.Api.Models;

namespace DailyGoalTracker.Api.Services;

public class TeamMemberService
{
    private readonly TeamMemberRepository _repository;

    public TeamMemberService(TeamMemberRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TeamMember>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<TeamMember?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<TeamMember> UpdateMoodAsync(int id, string? mood)
    {
        // Validate mood if provided
        if (mood != null && !IsValidMood(mood))
        {
            throw new ArgumentException($"Invalid mood value: {mood}. Must be one of: 😀, 😊, 😐, 😞, 😤");
        }

        var teamMember = await _repository.GetByIdAsync(id);
        if (teamMember == null)
        {
            throw new KeyNotFoundException($"Team member with id {id} not found");
        }

        await _repository.UpdateMoodAsync(id, mood);
        
        // Return updated team member
        return await _repository.GetByIdAsync(id) ?? teamMember;
    }

    private static bool IsValidMood(string mood)
    {
        return mood is "😀" or "😊" or "😐" or "😞" or "😤";
    }
}

