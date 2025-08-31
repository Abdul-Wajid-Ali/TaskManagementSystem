namespace TaskManagement.Application.Common
{
    public static class ErrorCodes
    {
        public const string UserNotFound = "USER_NOT_FOUND";
        public const string InvalidCredentials = "INVALID_CREDENTIALS";
        public const string InternalServerError = "INTERNAL_SERVER_ERROR";
        public const string UserEmailAlreadyExists = "USER_EMAIL_ALREADY_EXISTS";
    }
}