using System;

namespace TaskManagement.Domain.Entities
{
    /// <summary>
    ///     Stores information about API requests that resulted in an error response.
    /// </summary>
    public class ExceptionLog
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public string Method { get; set; } = default!;
        public string EndPoint { get; set; } = default!;
        public string ExceptionName { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string StackTrace { get; set; } = default!;
        public DateTime LoggedAt { get; set; }
    }
}