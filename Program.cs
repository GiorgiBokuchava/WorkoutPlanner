using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;
using WorkoutPlanner_API.Application.Interfaces;
using WorkoutPlanner_API.Infrastructure.Options;
using WorkoutPlanner_API.Infrastructure.Persistance;

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

// Register repositories
builder.Services.AddScoped<IUserRepository, SqlUserRepository>();
builder.Services.AddScoped<IRoutineRepository, SqlRoutineRepository>();
builder.Services.AddScoped<IExerciseRepository, SqlExerciseRepository>();
builder.Services.AddScoped<IWorkoutLogRepository, SqlWorkoutLogRepository>();

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