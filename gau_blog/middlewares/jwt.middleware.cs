using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace gau_blog.middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _jwtSecret;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _jwtSecret = configuration["JWT_SECRET"] ?? throw new InvalidOperationException("JWT_SECRET is missing from configuration.");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var tokenString = context.Request.Cookies["auth_token"];

            if (string.IsNullOrEmpty(tokenString))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Authorization cookie is required");
                return;
            }

            var principal = await ValidateToken(tokenString);

            if (principal == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid or expired token");
                return;
            }

            context.Items["UserId"] = principal.FindFirst("user_id")?.Value ?? string.Empty;
            context.Items["Permission"] = principal.FindFirst("permission")?.Value ?? string.Empty;
            context.Items["Fullname"] = principal.FindFirst("fullname")?.Value ?? string.Empty;

            await _next(context);
        }

        public Task<ClaimsPrincipal?> ValidateToken(string tokenString)
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
                return Task.FromResult<ClaimsPrincipal?>(principal);
            }
            catch
            {
                return Task.FromResult<ClaimsPrincipal?>(null);
            }
        }
    }
}
