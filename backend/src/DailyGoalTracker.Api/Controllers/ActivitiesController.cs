using Microsoft.AspNetCore.Mvc;
using DailyGoalTracker.Api.Models;
using DailyGoalTracker.Api.Services;

namespace DailyGoalTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly ActivityService _service;
    private readonly ILogger<ActivitiesController> _logger;

    public ActivitiesController(ActivityService service, ILogger<ActivitiesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ActivityRecord>>>> Get()
    {
        _logger.LogInformation("Fetching all activity records");
        var activities = await _service.GetAllAsync();
        return Ok(new ApiResponse<IEnumerable<ActivityRecord>>
        {
            Data = activities
        });
    }
}

