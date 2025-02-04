using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace gau_blog.services
{
    public class JwtService
    {
        private readonly string _jwtSecret;

        public JwtService(IConfiguration configuration)
        {
            _jwtSecret = configuration["JWT_SECRET"] ?? throw new InvalidOperationException("JWT_SECRET is missing from configuration.");
        }

        public ClaimsPrincipal? ValidateToken(string tokenString)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                    ValidateLifetime = true
                };

                var principal = tokenHandler.ValidateToken(tokenString, validationParameters, out _);
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}