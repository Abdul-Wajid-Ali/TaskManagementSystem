namespace TaskManagement.API.Responses
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }

        public string? ErrorCode { get; set; }

        public string? SuccessCode { get; set; }

        public T? Data { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string successCode = default!) =>
            string.IsNullOrWhiteSpace(successCode)
                ? new() { IsSuccess = true, Data = data }
                : new() { IsSuccess = true, Data = data, SuccessCode = successCode };

        public static ApiResponse<T> FailResponse(string errorCode) =>
            new() { IsSuccess = false, Data = default!, ErrorCode = errorCode };
    }
}