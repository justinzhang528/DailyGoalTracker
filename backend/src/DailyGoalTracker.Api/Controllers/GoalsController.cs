using Microsoft.AspNetCore.Mvc;
using DailyGoalTracker.Api.Models;
using DailyGoalTracker.Api.Services;

namespace DailyGoalTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GoalsController : ControllerBase
{
    private readonly GoalService _service;
    private readonly ActivityService _activityService;
    private readonly TeamMemberService _teamMemberService;
    private readonly ILogger<GoalsController> _logger;

    public GoalsController(GoalService service, ActivityService activityService, TeamMemberService teamMemberService, ILogger<GoalsController> logger)
    {
        _service = service;
        _activityService = activityService;
        _teamMemberService = teamMemberService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<Goal>>>> Get([FromQuery] int? teamMemberId)
    {
        var goals = await _service.GetAllAsync(teamMemberId);
        return Ok(new ApiResponse<IEnumerable<Goal>>
        {
            Data = goals
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<Goal>>> GetById(int id)
    {
        var goal = await _service.GetByIdAsync(id);
        if (goal == null)
        {
            return NotFound(new ApiResponse<Goal>
            {
                Error = $"Goal with id {id} not found"
            });
        }

        return Ok(new ApiResponse<Goal>
        {
            Data = goal
        });
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<Goal>>> Create([FromBody] CreateGoalRequest request)
    {
        try
        {
            _logger.LogInformation("Creating goal for team member {TeamMemberId}: {Description}", 
                request.TeamMemberId, request.Description);
            var goal = await _service.CreateAsync(request.TeamMemberId, request.Description);
            _logger.LogInformation("Goal created successfully with ID {GoalId}", goal.Id);
            
            // Record activity (best-effort, non-blocking)
            try
            {
                var teamMember = await _teamMemberService.GetByIdAsync(request.TeamMemberId);
                if (teamMember != null)
                {
                    var description = $"{teamMember.Name}'s goal '{goal.Description}' was added";
                    await _activityService.RecordActivityAsync(ActivityType.AddGoal, request.TeamMemberId, teamMember.Name, description, goal.Id, goal.Description);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to record activity for goal creation (non-blocking)");
            }
            
            return CreatedAtAction(nameof(GetById), new { id = goal.Id }, new ApiResponse<Goal>
            {
                Data = goal,
                Message = "Goal created successfully"
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Invalid request to create goal: {Error}", ex.Message);
            return BadRequest(new ApiResponse<Goal>
            {
                Error = ex.Message
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Team member not found: {Error}", ex.Message);
            return NotFound(new ApiResponse<Goal>
            {
                Error = ex.Message
            });
        }
    }

    [HttpPut("{id}/complete")]
    public async Task<ActionResult<ApiResponse<Goal>>> UpdateComplete(int id, [FromBody] UpdateGoalCompleteRequest request)
    {
        try
        {
            _logger.LogInformation("Updating goal {GoalId} completion status to {IsComplete}", id, request.IsComplete);
            var goal = await _service.UpdateCompleteStatusAsync(id, request.IsComplete);
            
            // Only record activity when goal is marked complete (not when uncompleting)
            if (request.IsComplete)
            {
                try
                {
                    var teamMember = await _teamMemberService.GetByIdAsync(goal.TeamMemberId);
                    if (teamMember != null)
                    {
                        var description = $"{teamMember.Name}'s goal '{goal.Description}' was completed";
                        await _activityService.RecordActivityAsync(ActivityType.CompleteGoal, goal.TeamMemberId, teamMember.Name, description, goal.Id, goal.Description);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to record activity for goal completion (non-blocking)");
                }
            }
            
            return Ok(new ApiResponse<Goal>
            {
                Data = goal,
                Message = "Goal status updated successfully"
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Goal not found: {Error}", ex.Message);
            return NotFound(new ApiResponse<Goal>
            {
                Error = ex.Message
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            _logger.LogInformation("Deleting goal {GoalId}", id);
            
            // Get goal info before deletion for activity recording
            var goal = await _service.GetByIdAsync(id);
            TeamMember? teamMember = null;
            if (goal != null)
            {
                teamMember = await _teamMemberService.GetByIdAsync(goal.TeamMemberId);
            }
            
            await _service.DeleteAsync(id);
            _logger.LogInformation("Goal {GoalId} deleted successfully", id);
            
            // Record activity (best-effort, non-blocking)
            if (goal != null && teamMember != null)
            {
                try
                {
                    var description = $"{teamMember.Name}'s goal '{goal.Description}' was deleted";
                    await _activityService.RecordActivityAsync(ActivityType.DeleteGoal, goal.TeamMemberId, teamMember.Name, description, goal.Id, goal.Description);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to record activity for goal deletion (non-blocking)");
                }
            }
            
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Goal not found for deletion: {Error}", ex.Message);
            return NotFound(new ApiResponse<object>
            {
                Error = ex.Message
            });
        }
    }
}

public class CreateGoalRequest
{
    public int TeamMemberId { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class UpdateGoalCompleteRequest
{
    public bool IsComplete { get; set; }
}

