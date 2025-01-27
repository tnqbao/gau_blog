using System.Text.Json;
using gau_blog.config;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<JsonSerializerOptions>(JsonSerializerConfiguration.GetJsonSerializerOptions());

var app = builder.Build();
app.MapGet("/", () => "Hello World!");

app.Run();