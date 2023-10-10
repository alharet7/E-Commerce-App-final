using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace E_Commerce_App.Models.Services
{
    public class JWTTokenService
    {

        private IConfiguration configuration;
        private SignInManager<ApplicationUser> signInManager;
        public JWTTokenService(IConfiguration config, SignInManager<ApplicationUser> manager)
        {
            configuration = config;
            signInManager = manager;
        }

        /// <summary>
        /// Get the validation parameters for JWT token validation.
        /// </summary>
        /// <param name="configuration">Configuration object.</param>
        public static TokenValidationParameters GetValidationPerameters(IConfiguration configuration)
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSecurityKey(configuration),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        }

        /// <summary>
        /// Get the security key for generating JWT tokens.
        /// </summary>
        /// <param name="configuration">Configuration object.</param>
        private static SecurityKey GetSecurityKey(IConfiguration configuration)
        {
            var secret = configuration["JWT:Secret"];
            if (secret == null)
            {
                throw new InvalidOperationException("JWT: Secret key is not exist");
            }

            var secretBytes = Encoding.UTF8.GetBytes(secret);
            return new SymmetricSecurityKey(secretBytes);
        }

        /// <summary>
        /// Generate a JWT token for the given user.
        /// </summary>
        /// <param name="user">User for whom the token is generated.</param>
        /// <param name="expiresIn">Token expiration time.</param>
        public async Task<string> GetToken(ApplicationUser user, TimeSpan expiresIn)
        {
            var principle = await signInManager.CreateUserPrincipalAsync(user);

            if (principle == null)
            {
                return null;
            }
            var signingKey = GetSecurityKey(configuration);

            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow + expiresIn,
                signingCredentials: new SigningCredentials(signingKey,
                SecurityAlgorithms.HmacSha256),
                claims: principle.Claims

                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
