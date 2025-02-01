using DotNetEnv;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace gau_blog.middlewares;
public class CORSMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string[] _allowedOrigins;

    public CORSMiddleware(RequestDelegate next)
    {
        _next = next;

        Env.Load();
        var domains = Env.GetString("LIST_DOMAIN", "*");
        _allowedOrigins = domains.Split('^');  
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", string.Join(",", _allowedOrigins));
        context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
        context.Response.Headers.Add("Access-Control-Allow-Headers", "Origin, Content-Type, Authorization");
        context.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Length, Authorization, Set-Cookie");
        context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
        context.Response.Headers.Add("Access-Control-Max-Age", "43200");  

        if (context.Request.Method == HttpMethods.Options)
        {
            context.Response.StatusCode = StatusCodes.Status204NoContent;
            return;
        }

        await _next(context);
    }
}