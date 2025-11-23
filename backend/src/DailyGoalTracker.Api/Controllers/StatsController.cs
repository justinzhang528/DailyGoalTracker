using Microsoft.AspNetCore.Mvc;
using DailyGoalTracker.Api.Models;
using DailyGoalTracker.Api.Services;

namespace DailyGoalTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatsController : ControllerBase
{
    private readonly StatsService _service;
    private readonly ILogger<StatsController> _logger;

    public StatsController(StatsService service, ILogger<StatsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<TeamStats>>> Get()
    {
        _logger.LogInformation("Fetching team statistics");
        var stats = await _service.GetTeamStatsAsync();
        _logger.LogInformation("Team stats: {TotalGoals} total, {CompletedGoals} completed, {Percentage}%", 
            stats.TotalGoals, stats.CompletedGoals, stats.CompletionPercentage);
        return Ok(new ApiResponse<TeamStats>
        {
            Data = stats
        });
    }
}

