namespace TaskManagement.API.Responses
{
    public class ApiResponse<T>
    {
        public bool isSuccess { get; set; }

        public string? errorCode { get; set; }

        public string? successCode { get; set; }

        public T? data { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string successCode = default!) =>
            string.IsNullOrWhiteSpace(successCode) ?
            new() { isSuccess = true, data = data } :
            new() { isSuccess = true, data = data, successCode = successCode };

        public static ApiResponse<T> FailResponse(string errorCode) =>
            new() { isSuccess = false, data = default!, errorCode = errorCode };
    }
}