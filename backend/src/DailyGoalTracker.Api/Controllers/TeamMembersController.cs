using Microsoft.AspNetCore.Mvc;
using DailyGoalTracker.Api.Models;
using DailyGoalTracker.Api.Services;

namespace DailyGoalTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamMembersController : ControllerBase
{
    private readonly TeamMemberService _service;
    private readonly ActivityService _activityService;
    private readonly ILogger<TeamMembersController> _logger;

    public TeamMembersController(TeamMemberService service, ActivityService activityService, ILogger<TeamMembersController> logger)
    {
        _service = service;
        _activityService = activityService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<TeamMember>>>> Get()
    {
        var teamMembers = await _service.GetAllAsync();
        return Ok(new ApiResponse<IEnumerable<TeamMember>>
        {
            Data = teamMembers
        });
    }

    [HttpPut("{id}/mood")]
    public async Task<ActionResult<ApiResponse<TeamMember>>> UpdateMood(int id, [FromBody] UpdateMoodRequest request)
    {
        try
        {
            _logger.LogInformation("Updating mood for team member {TeamMemberId} to {Mood}", id, request.Mood);
            var teamMember = await _service.UpdateMoodAsync(id, request.Mood);
            _logger.LogInformation("Mood updated successfully for team member {TeamMemberId}", id);
            
            // Record activity (best-effort, non-blocking)
            if (request.Mood != null)
            {
                try
                {
                    var description = $"{teamMember.Name}'s mood was updated to {request.Mood}";
                    await _activityService.RecordActivityAsync(ActivityType.UpdateMood, id, teamMember.Name, description);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to record activity for mood update (non-blocking)");
                }
            }
            
            return Ok(new ApiResponse<TeamMember>
            {
                Data = teamMember,
                Message = "Mood updated successfully"
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Team member not found: {Error}", ex.Message);
            return NotFound(new ApiResponse<TeamMember>
            {
                Error = ex.Message
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Invalid mood value: {Error}", ex.Message);
            return BadRequest(new ApiResponse<TeamMember>
            {
                Error = ex.Message
            });
        }
    }
}

public class UpdateMoodRequest
{
    public string? Mood { get; set; }
}

