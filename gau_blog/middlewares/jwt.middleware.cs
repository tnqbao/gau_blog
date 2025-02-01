using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNetEnv;

namespace gau_blog.middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _jwtSecret;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _jwtSecret = Env.GetString("JWT_SECRET");  // Lấy key từ biến môi trường
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

            // Lấy claims từ token
            var claims = principal.Claims.ToList();

            // Gán thông tin từ claims vào context
            var userIdClaim = claims.FirstOrDefault(c => c.Type == "user_id")?.Value;
            var permissionClaim = claims.FirstOrDefault(c => c.Type == "permission")?.Value;
            var fullnameClaim = claims.FirstOrDefault(c => c.Type == "fullname")?.Value;

            if (userIdClaim != null)
            {
                context.Items["UserId"] = userIdClaim;
            }

            if (permissionClaim != null)
            {
                context.Items["Permission"] = permissionClaim;
            }

            if (fullnameClaim != null)
            {
                context.Items["Fullname"] = fullnameClaim;
            }

            await _next(context); // Tiếp tục xử lý request
        }

        // Phương thức xác thực token
        public Task<ClaimsPrincipal?> ValidateToken(string tokenString)
        {
            var key = Encoding.UTF8.GetBytes(_jwtSecret); // Khóa bí mật

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                
                // Không in khóa bí mật vào console để tránh rủi ro bảo mật
                Console.WriteLine("Token: " + tokenString);
                
                // Xác thực token
                var principal = tokenHandler.ValidateToken(tokenString, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key), // Sử dụng khóa bí mật
                    ValidateIssuer = false,  // Không kiểm tra Issuer (có thể thay đổi nếu cần)
                    ValidateAudience = false,  // Không kiểm tra Audience (có thể thay đổi nếu cần)
                    ClockSkew = TimeSpan.Zero  // Không có độ trễ thời gian
                }, out _);

                return Task.FromResult(principal); // Trả về ClaimsPrincipal nếu token hợp lệ
            }
            catch (Exception ex)
            {
                // In thông báo lỗi chi tiết nếu không hợp lệ
                Console.WriteLine("Token validation failed: " + ex.Message);
                return Task.FromResult<ClaimsPrincipal?>(null);  // Trả về null nếu không hợp lệ
            }
        }
    }
}
