using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace gau_blog.middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _secret;

    public JwtMiddleware(RequestDelegate next, string secret)
    {
        _next = next;
        _secret = secret;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                var userId = ValidateJwtToken(token);
                if (userId != null)
                {
                    context.Items["UserId"] = userId;
                }
            }
            catch
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid or expired token.");
                return;
            }
        }

        await _next(context);
    }

    private string? ValidateJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secret);

        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;

        return userId;
    }
}