using gringotts_application.Exceptions;
using gringotts_application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace gringotts_application.Services.Imp
{
    /// <summary>
    /// Implementation of the authentication service interface (IAuthService).
    /// </summary>
    public class AuthService : IAuthService
    {
        /// <summary>
        /// Implementación concreta de la interfaz IAuthService que proporciona funcionalidades relacionadas con la autenticación.
        /// </summary>
        private readonly GringottsDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _salt;

        /// <summary>
        /// Constructor for AuthServiceImp.
        /// </summary>
        /// <param name="context">Database context for Gringotts.</param>
        /// <param name="configuration">Configuration for authentication settings.</param>
        public AuthService(GringottsDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _salt = _configuration.GetSection("AuthenticationSettings:Salt").Value;
        }


        /// <summary>
        /// Generates a JWT token based on user login information.
        /// </summary>
        /// <param name="userLoginModel">User login data.</param>
        /// <returns>JWT token if authentication is successful, otherwise null.</returns>
        public async Task<string> GenerateToken(UserLoginModel userLoginModel)
        {
            
            var userFind = await _context.users.FirstOrDefaultAsync(u => u.name == userLoginModel.name);
            
            var encodedJwt = "";

            if (userFind != null && VerifyPassword(userLoginModel.password, userFind.password, _salt))
            {
                var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,userFind.iduser.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim("name",userFind.name),
                    new Claim("role", userFind.role),
                    new Claim("profile_picture", userFind.profile_picture)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AuthenticationSettings:SigningKey").Value));
                var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var actualDate = DateTime.UtcNow;
                var expireDate = DateTime.UtcNow.AddDays(220);
                var jwt = new JwtSecurityToken(
                    issuer: "Peticionario",
                    audience: "Public",
                    claims: claims,
                    notBefore: actualDate,
                    expires: expireDate,
                    signingCredentials: credential
                );
                encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                return encodedJwt;
            }
            else
            {
                var msg = "Invalid Credentials";
                throw new ApiException(msg);
            }
        } 

        /// <summary>
        /// Validates a JWT token.
        /// </summary>
        /// <param name="token">JWT token to be validated.</param>
        /// <returns>True if the token is valid, false otherwise.</returns>
        /// 
        public async Task<bool> ValidatedToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("AuthenticationSettings:SigningKey").Value);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = "Peticionario",
                    ValidAudience = "Public",
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                var msg = "Invalid token";
                throw new ApiException(msg);
            }
        }
        /// <summary>
        /// Verifies if a provided password matches a stored password using a salt value.
        /// </summary>
        /// <param name="inputPassword">Password provided by the user for verification.</param>
        /// <param name="storedPassword">Password stored in the database.</param>
        /// <param name="salt">Salt value used to enhance password security.</param>
        /// <returns>True if the provided password matches the stored password; otherwise, false.</returns>
        public bool VerifyPassword(string inputPassword, string storedPassword, string _salt)
        {

            string Salt = _salt.Substring(0, 64);

            var hashedInputPassword = DataEncoder(inputPassword, Salt);
            return hashedInputPassword == storedPassword;
        }

        /// <summary>
        /// Encodes data (e.g., password) using hashing algorithm with a salt value.
        /// </summary>
        /// <param name="varToEncrypt">Data (e.g., password) to be encoded.</param>
        /// <param name="Salt">Salt value used to enhance data security.</param>
        /// <returns>Encoded data with salt.</returns>
        public string DataEncoder(string varToEncrypt, string Salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(varToEncrypt);
                byte[] saltBytes = Encoding.UTF8.GetBytes(Salt);

                byte[] combinedBytes = new byte[saltBytes.Length + passwordBytes.Length];
                Buffer.BlockCopy(saltBytes, 0, combinedBytes, 0, saltBytes.Length);
                Buffer.BlockCopy(passwordBytes, 0, combinedBytes, saltBytes.Length, passwordBytes.Length);

                byte[] hashedBytes = sha256.ComputeHash(combinedBytes);

                string hashedPasswordWithSalt = Convert.ToHexString(Encoding.UTF8.GetBytes(Convert.ToHexString(hashedBytes) + saltBytes));

                return hashedPasswordWithSalt;
            }
        }

    }
}