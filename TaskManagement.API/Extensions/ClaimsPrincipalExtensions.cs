using System.Security.Claims;

namespace TaskManagement.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Gets the current user's ID from claims.
        /// </summary>
        /// <param name="user">The claims principal.</param>
        /// <returns>User ID as long if found, otherwise null.</returns>
        public static long? GetCurrentUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.Sid) ?? user.FindFirst("Id");

            if (userIdClaim == null)
                return null;

            if (long.TryParse(userIdClaim.Value, out var userId))
                return userId;

            return null;
        }

        /// <summary>
        /// Gets the current user's Role from claims.
        /// </summary>
        /// <param name="user">The claims principal.</param>
        /// <returns>User Role as long if found, otherwise null.</returns>
        public static string? GetCurrentUserRole(this ClaimsPrincipal user)
        {
            if (user == null)
                return null;

            var userRoleClaim = user.FindFirst(ClaimTypes.Email) ?? user.FindFirst("Email");

            if (userRoleClaim != null)
                return userRoleClaim.Value;

            return null;
        }
    }
}