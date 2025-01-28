using System.Text.Json;
using gau_blog.apis;
using gau_blog.config;
using gau_blog.middlewares;
using gau_blog.routes;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureDatabase();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<JsonSerializerOptions>(JsonSerializerConfiguration.GetJsonSerializerOptions());
builder.Services.AddSingleton<JwtMiddleware>();
builder.Services.AddScoped<JwtMiddleware>();
builder.Services.AddScoped<BlogApi>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}


Routes.MapRoutes(app);

app.Run();