using gringotts_application.DTOs;

namespace gringotts_application.Services
{
    /// <summary>
    /// Interface for user service.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Attempts to log in a user with the provided username and password.
        /// </summary>
        /// <param name="username">User's username.</param>
        /// <param name="password">User's password.</param>
        /// <returns>User entity if login is successful, otherwise null.</returns>
        public UserLoginDTO LoginUser(string username, string password);
    }
}
