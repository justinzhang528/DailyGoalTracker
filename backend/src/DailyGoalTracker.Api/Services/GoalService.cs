using DailyGoalTracker.Api.Data.Dapper;
using DailyGoalTracker.Api.Models;

namespace DailyGoalTracker.Api.Services;

public class GoalService
{
    private readonly GoalRepository _repository;
    private readonly TeamMemberRepository _teamMemberRepository;

    public GoalService(GoalRepository repository, TeamMemberRepository teamMemberRepository)
    {
        _repository = repository;
        _teamMemberRepository = teamMemberRepository;
    }

    public async Task<IEnumerable<Goal>> GetAllAsync(int? teamMemberId = null)
    {
        return await _repository.GetAllAsync(teamMemberId);
    }

    public async Task<Goal?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Goal> CreateAsync(int teamMemberId, string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Goal description cannot be empty", nameof(description));
        }

        if (description.Length > 500)
        {
            throw new ArgumentException("Goal description cannot exceed 500 characters", nameof(description));
        }

        // Validate team member exists
        var teamMember = await _teamMemberRepository.GetByIdAsync(teamMemberId);
        if (teamMember == null)
        {
            throw new KeyNotFoundException($"Team member with id {teamMemberId} not found");
        }

        var goal = new Goal
        {
            TeamMemberId = teamMemberId,
            Description = description.Trim(),
            IsComplete = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var id = await _repository.CreateAsync(goal);
        goal.Id = id;
        
        return goal;
    }

    public async Task<Goal> UpdateCompleteStatusAsync(int id, bool isComplete)
    {
        var goal = await _repository.GetByIdAsync(id);
        if (goal == null)
        {
            throw new KeyNotFoundException($"Goal with id {id} not found");
        }

        goal.IsComplete = isComplete;
        goal.UpdatedAt = DateTime.UtcNow;
        
        await _repository.UpdateAsync(goal);
        
        return goal;
    }

    public async Task DeleteAsync(int id)
    {
        var goal = await _repository.GetByIdAsync(id);
        if (goal == null)
        {
            throw new KeyNotFoundException($"Goal with id {id} not found");
        }

        await _repository.DeleteAsync(id);
    }
}

