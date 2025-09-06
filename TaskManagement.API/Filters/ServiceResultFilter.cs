using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManagement.API.Responses;
using TaskManagement.Application.Common;

namespace TaskManagement.API.Filters
{
    /// <summary>
    /// Translates Result<T> returned from actions into <see cref="ApiResponse{T}"/>
    /// and maps success or error codes to appropriate HTTP results.
    /// </summary>
    public class ServiceResultFilter : IAsyncActionFilter
    {
        /// <summary>
        /// Executes the action and, after it completes, inspects the result for a
        /// service Result<T> When found, it is converted into an
        /// <see cref="ApiResponse{T}"/> and the <see cref="IActionResult"/> is updated.
        /// </summary>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Execute the action
            await next();

            // Only handle ObjectResult with a non-null value
            if (context.Result is not ObjectResult objectResult || objectResult.Value is null)
                return;

            // Only handle results of type Result<T>
            var valueType = objectResult.Value.GetType();
            if (!valueType.IsGenericType || valueType.GetGenericTypeDefinition() != typeof(Result<>))
                return;

            // Extract Result<T> properties using reflection since T is unknown at compile time
            var isSuccess = (bool)valueType.GetProperty(nameof(Result<object>.IsSuccess))!.GetValue(objectResult.Value)!;
            var errorCode = (string?)valueType.GetProperty(nameof(Result<object>.ErrorCode))!.GetValue(objectResult.Value);
            var successCode = (string?)valueType.GetProperty(nameof(Result<object>.SuccessCode))!.GetValue(objectResult.Value);
            var data = valueType.GetProperty(nameof(Result<object>.Data))!.GetValue(objectResult.Value);

            // Build a generic ApiResponse<T> matching the service result's data type
            var dataType = valueType.GetGenericArguments()[0];
            var apiResponseType = typeof(ApiResponse<>).MakeGenericType(dataType);

            if (isSuccess)
            {
                // Success - return 200 OK with ApiResponse<T>
                var method = apiResponseType.GetMethod(nameof(ApiResponse<object>.SuccessResponse));
                var apiResponse = method!.Invoke(null, [data, successCode]);

                context.Result = new OkObjectResult(apiResponse);
            }
            else
            {
                // Failure - map error code to appropriate HTTP status
                var method = apiResponseType.GetMethod(nameof(ApiResponse<object>.FailResponse));
                var apiResponse = method!.Invoke(null, [errorCode]);

                if (!string.IsNullOrWhiteSpace(errorCode) && errorCode.Contains("NOT_FOUND", StringComparison.OrdinalIgnoreCase))
                    context.Result = new NotFoundObjectResult(apiResponse);
                else if (!string.IsNullOrWhiteSpace(errorCode) && errorCode.Contains("ALREADY_EXISTS", StringComparison.OrdinalIgnoreCase))
                    context.Result = new ConflictObjectResult(apiResponse);
                else
                    context.Result = new BadRequestObjectResult(apiResponse);
            }
        }
    }
}