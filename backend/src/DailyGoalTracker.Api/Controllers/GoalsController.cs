using Microsoft.AspNetCore.Mvc;
using DailyGoalTracker.Api.Models;
using DailyGoalTracker.Api.Services;

namespace DailyGoalTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GoalsController : ControllerBase
{
    private readonly GoalService _service;
    private readonly ILogger<GoalsController> _logger;

    public GoalsController(GoalService service, ILogger<GoalsController> logger)
    {
        _service = service;
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
            await _service.DeleteAsync(id);
            _logger.LogInformation("Goal {GoalId} deleted successfully", id);
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

