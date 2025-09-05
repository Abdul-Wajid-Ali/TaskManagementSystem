using System.Net;
using System.Text.Json;
using TaskManagement.API.Extensions;
using TaskManagement.Application.Interfaces.Logging;
using TaskManagement.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.API.Middleware
{
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IExceptionLogWriter _writer;
        private readonly ILogger<ExceptionLoggingMiddleware> _logger;

        public ExceptionLoggingMiddleware(
            RequestDelegate next,
            IExceptionLogWriter writer,
            ILogger<ExceptionLoggingMiddleware> logger)
        {
            _next = next;
            _writer = writer;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Best-effort: capture info before response is written
                var userId = context.User.GetCurrentUserId();
                var method = context.Request.Method;
                var endpoint = context.Request.Path.HasValue ? context.Request.Path.Value! : string.Empty;

                // If nothing was written yet, make sure we return a 500
                var statusCode = context.Response.HasStarted ? context.Response.StatusCode : (int)HttpStatusCode.InternalServerError;

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
                await _writer.WriteAsync(log);

                // Also log to app logs (observability)
                _logger.LogError(ex,
                    "Unhandled exception at {Method} {Endpoint}. UserId={UserId}, StatusCode={StatusCode}, TraceId={TraceId}",
                    method, endpoint, userId?.ToString() ?? "anonymous", statusCode, context.TraceIdentifier);

                // Craft a controlled error response if possible
                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var payload = new
                    {
                        title = "An unexpected error occurred.",
                        status = context.Response.StatusCode,
                        traceId = context.TraceIdentifier
                    };

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
