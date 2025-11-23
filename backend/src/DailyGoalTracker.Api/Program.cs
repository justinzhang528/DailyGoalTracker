using DailyGoalTracker.Api.Data.Database;
using DailyGoalTracker.Api.Middleware;
using DailyGoalTracker.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Register services
builder.Services.AddScoped<TeamMemberConfigService>();

var app = builder.Build();

// Initialize database
DatabaseInitializer.Initialize();

// Load team members from config
using (var scope = app.Services.CreateScope())
{
    var configService = scope.ServiceProvider.GetRequiredService<TeamMemberConfigService>();
    await configService.LoadTeamMembersAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();
