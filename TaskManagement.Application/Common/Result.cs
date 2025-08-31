namespace TaskManagement.Application.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; init; }

        public string? SuccessCode { get; init; }

        public string ErrorCode { get; init; } = default!;

        public T? Data { get; init; }

        public static Result<T> Success(T data, string successCode = default!) =>
            string.IsNullOrWhiteSpace(successCode) ?
            new() { IsSuccess = true, Data = data, } :
            new() { IsSuccess = true, SuccessCode = successCode, Data = data, };

        public static Result<T> Fail(string error) => new() { IsSuccess = false, ErrorCode = error };
    }
}