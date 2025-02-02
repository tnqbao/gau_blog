using System.Text.Json;
using DotNetEnv;
using gau_blog.apis;
using gau_blog.config;
using gau_blog.middlewares;
using gau_blog.repositories;
using gau_blog.routes;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register services in DI container
builder.Services.ConfigureDatabase();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<JsonSerializerOptions>(JsonSerializerConfiguration.GetJsonSerializerOptions());
builder.Services.AddScoped<BlogRepository>();
builder.Services.AddScoped<BlogApi>();

// Ensure JWT secret is loaded
Env.Load();

var app = builder.Build();

// Add middleware
app.UseMiddleware<JwtMiddleware>();  // Make sure this is before other routes or authentication

// Migrate the database if needed
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Add routes
Routes.MapRoutes(app);

app.Run();
