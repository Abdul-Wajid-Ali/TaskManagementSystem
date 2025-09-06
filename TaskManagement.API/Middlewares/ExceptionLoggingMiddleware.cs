using System.Net;
using System.Text.Json;
using TaskManagement.API.Extensions;
using TaskManagement.Application.Interfaces.Logging;
using TaskManagement.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.API.Middleware
{
    /// <summary>
    /// Middleware to catch unhandled exceptions, log them, and return a generic error response.
    /// </summary>
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionLoggingMiddleware> _logger;

        public ExceptionLoggingMiddleware(RequestDelegate next, ILogger<ExceptionLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // This method is called for each HTTP request after the previous middleware in the pipeline.
        public async Task InvokeAsync(HttpContext context, IExceptionLogWriter writer)
        {
            try
            {
                // Proceed down the pipeline, if exception occurs, we jump to catch block
                await _next(context);
            }
            catch (Exception ex)
            {
                // Gather required info for logging
                var userId = context.User.GetCurrentUserId();
                var method = context.Request.Method;
                var endpoint = context.Request.Path.HasValue ? context.Request.Path.Value! : string.Empty;

                // If nothing was written yet, make sure we return a 500
                var statusCode = context.Response.HasStarted ? context.Response.StatusCode : (int)HttpStatusCode.InternalServerError;

                // Create log entry
                var log = new ExceptionLog
                {
                    UserId = userId,
                    Method = method,
                    EndPoint = endpoint,
                    ExceptionName = ex.GetType().FullName ?? ex.GetType().Name,
                    Message = ex.Message,
                    StackTrace = ex.ToString(),
                    LoggedAt = DateTime.UtcNow
                };

                // Write to DB (don’t let failures here crash the request)
                await writer.WriteAsync(log);

                // Also log to app logs (observability)
                _logger.LogError(ex,
                    "Unhandled exception at {Method} {Endpoint}. UserId={UserId}, StatusCode={StatusCode}, TraceId={TraceId}",
                    method, endpoint, userId?.ToString() ?? "anonymous", statusCode, context.TraceIdentifier);

                // If response has not started, we can write a generic 500 response
                if (!context.Response.HasStarted)
                {
                    // Clear any existing response
                    context.Response.Clear();
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    // Return minimal info to avoid leaking details
                    var payload = new
                    {
                        title = "An unexpected error occurred.",
                        status = context.Response.StatusCode,
                        traceId = context.TraceIdentifier
                    };

                    // Serialize and write response
                    await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
                }
                else
                {
                    // If headers/body already started, just rethrow
                    throw;
                }
            }
        }
    }
}
