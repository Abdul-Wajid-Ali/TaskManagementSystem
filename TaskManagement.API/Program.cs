using Hangfire;
using TaskManagement.API.Extensions;
using TaskManagement.API.Middleware;
using TaskManagement.Application.Interfaces.Services;

var builder = WebApplication.CreateBuilder(args);

// Register all application services (DbContext, Auth, Repos, etc.)
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// 1. Exception handling first
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // catch unhandled exceptions
    app.MapOpenApi();                // dev-only API docs
}

// 2. Security & static handling
app.UseHttpsRedirection();

// 3. AuthN & AuthZ
app.UseAuthentication();
app.UseMiddleware<ExceptionLoggingMiddleware>();
app.UseAuthorization();

// 4. Controllers / endpoints
app.MapControllers();

// 5. Hangfire Dashboard & Recurring Jobs
app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<ITaskCleanupService>(
    "completed-task-cleanup",
    service => service.MarkCompletedTasksAsDeletedAsync(),
    Cron.Minutely());

await app.RunAsync();