namespace DailyGoalTracker.Api.Models;

public class ApiResponse<T>
{
    public T? Data { get; set; }
    public string? Message { get; set; }
    public string? Error { get; set; }
    public string? Details { get; set; }
}

