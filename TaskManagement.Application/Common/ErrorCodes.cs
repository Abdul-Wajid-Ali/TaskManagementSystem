namespace TaskManagement.Application.Common
{
    public static class ErrorCodes
    {
        // Error Code Users
        public const string UserNotFound = "USER_NOT_FOUND";
        public const string InvalidCredentials = "INVALID_CREDENTIALS";
        public const string InternalServerError = "INTERNAL_SERVER_ERROR";
        public const string UserEmailAlreadyExists = "USER_EMAIL_ALREADY_EXISTS";

        // Error Code Tasks
        public const string TaskNotFound = "TASK_NOT_FOUND";
        public const string TaskCreationFailed = "TASK_CREATION_FAILED";
        public const string TaskUpdateFailed = "TASK_UPDATE_FAILED";
        public const string TaskDeletionFailed = "TASK_DELETION_FAILED";
    }
}