using gringotts_application.Models;

namespace gringotts_application.Services
{
    /// <summary>
    /// Interface for authentication service.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Generates a JWT token based on user login information.
        /// </summary>
        /// <param name="userLoginModel">User login data.</param>
        /// <returns>JWT token if authentication is successful, otherwise null.</returns>
        Task<string> GenerateToken(UserLoginModel userLoginModel);

       /// <summary>
       /// Validates a JWT token.
       /// </summary>
       /// <param name="token">JWT token to be validated.</param>
       /// <returns>True if the token is valid, false otherwise.</returns>
       Task<bool> ValidatedToken(string token);
    }
}
