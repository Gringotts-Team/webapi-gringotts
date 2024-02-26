using gringotts_application.Exceptions;
using gringotts_application.Models;
using gringotts_application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gringotts_api.Controllers
{
    /// <summary>
    /// Controller for managing user-related
    /// </summary>
    [ApiController]
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Constructor for UserController.
        /// </summary>
        /// <param name="authService">Service for handling authentication</param>
        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Logs in a user and returns a JWT token upon successful authentication.
        /// </summary>
        /// <param name="loginDTO">User login data.</param>
        /// <returns>JWT token upon successful login.</returns>
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> PostLoginUser(UserLoginModel userLoginModel)
        {
            try
            {
                var newToken = await _authService.GenerateToken(userLoginModel);
                return Ok(new
                {
                    Token = newToken,
                });
            }
            catch (ApiException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("An error has occurred");
            }
        }

        /// <summary>
        /// Validates a JWT token.
        /// </summary>
        /// <param name="token">JWT token to be validated.</param>
        /// <returns>True if the token is valid, false otherwise.</returns>
        [HttpGet]
        [Route("tokenvalidation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Overseer, Minion")]
        public async Task<ActionResult<bool>> GetTokenValidated(string token)
        {
            try
            {
                var tokenValidated = await _authService.ValidatedToken(token);
                return Ok(tokenValidated);
            }
            catch (ApiException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("An error has occurred");
            }
        }
    }
}
