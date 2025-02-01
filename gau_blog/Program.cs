using System.Text.Json;
using DotNetEnv;
using gau_blog.apis;
using gau_blog.config;
using gau_blog.middlewares;
using gau_blog.repositories;
using gau_blog.routes;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureDatabase();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<JsonSerializerOptions>(JsonSerializerConfiguration.GetJsonSerializerOptions());
builder.Services.AddScoped<BlogRepository>();
builder.Services.AddScoped<BlogApi>();

builder.WebHost.UseUrls("http://*:7008");
Env.Load();
var app = builder.Build();

app.UseMiddleware<JwtMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

Routes.MapRoutes(app);

app.Run();