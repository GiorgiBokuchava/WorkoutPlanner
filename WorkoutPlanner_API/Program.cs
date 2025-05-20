using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.Extensions.Options;
using WorkoutPlanner_API.Infrastructure.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOptions<SwaggerSettings>()
    .BindConfiguration(SwaggerSettings.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    var swagger = app.Services.GetRequiredService<IOptions<SwaggerSettings>>().Value;
    app.UseSwagger();
    app.UseSwaggerUI(ui =>
    {
        ui.SwaggerEndpoint($"/swagger/{swagger.Version}/swagger.json",
                           $"{swagger.Title} {swagger.Version}");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();