using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace SwipSwapMarketplace.Auth
{
    /// <summary>
    /// Handles creation and validation of JSON Web Tokens (JWT) used for authentication.
    /// This service uses the <see cref="JwtSettings"/> configuration to generate secure tokens.
    /// </summary>
    public class JwtService
    {
        private readonly JwtSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtService"/> class with configured JWT options.
        /// </summary>
        /// <param name="settings">Injected JWT configuration from <c>appsettings.json</c>.</param>
        public JwtService(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Generates a signed JWT token containing the user’s ID and username as claims.
        /// </summary>
        /// <param name="userId">Unique identifier of the authenticated user.</param>
        /// <param name="username">Username of the authenticated user.</param>
        /// <returns>A signed JWT string.</returns>
        public string GenerateToken(string userId, string username)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.ExpireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
