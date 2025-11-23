using DailyGoalTracker.Api.Data.Dapper;
using DailyGoalTracker.Api.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace DailyGoalTracker.Api.Services;

public class StatsService
{
    private readonly TeamMemberRepository _teamMemberRepository;
    private readonly GoalRepository _goalRepository;

    public StatsService(TeamMemberRepository teamMemberRepository, GoalRepository goalRepository)
    {
        _teamMemberRepository = teamMemberRepository;
        _goalRepository = goalRepository;
    }

    public async Task<TeamStats> GetTeamStatsAsync()
    {
        var goals = await _goalRepository.GetAllAsync();
        var teamMembers = await _teamMemberRepository.GetAllAsync();

        var totalGoals = goals.Count();
        var completedGoals = goals.Count(g => g.IsComplete);
        var completionPercentage = totalGoals > 0 
            ? Math.Round((double)completedGoals / totalGoals * 100, 1) 
            : 0.0;

        var moodCounts = new Dictionary<string, int>
        {
            { "😀", 0 },
            { "😊", 0 },
            { "😐", 0 },
            { "😞", 0 },
            { "😤", 0 }
        };

        foreach (var member in teamMembers)
        {
            if (!string.IsNullOrEmpty(member.CurrentMood) && moodCounts.ContainsKey(member.CurrentMood))
            {
                moodCounts[member.CurrentMood]++;
            }
        }

        return new TeamStats
        {
            TotalGoals = totalGoals,
            CompletedGoals = completedGoals,
            CompletionPercentage = completionPercentage,
            MoodCounts = moodCounts
        };
    }
}

