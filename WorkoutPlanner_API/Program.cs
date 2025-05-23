using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using WorkoutPlanner_API.Infrastructure.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Bind & validate settings DTO
builder.Services.AddOptions<SwaggerSettings>()
    .BindConfiguration(SwaggerSettings.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Register Swashbuckle and your ConfigureOptions class
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
//     Resolve validated settings and wire up the UI
    var swaggerSettings = app.Services
                             .GetRequiredService<IOptions<SwaggerSettings>>()
                             .Value;

    app.UseSwagger();
    app.UseSwaggerUI(ui =>
    {
        ui.SwaggerEndpoint(
            $"/swagger/{swaggerSettings.Version}/swagger.json",
            $"{swaggerSettings.Title} {swaggerSettings.Version}"
        );
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

static Uri? UriOrNull(string? url)
{
    if (string.IsNullOrWhiteSpace(url))
        return null;

    return new Uri(url);
}
