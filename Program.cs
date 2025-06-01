using Application.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using WorkoutPlanner.Application.Interfaces;
using WorkoutPlanner.Application.Services;
using WorkoutPlanner.Infrastructure.Options;
using WorkoutPlanner.Infrastructure.Persistance;
using WorkoutPlanner.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Bind & validate Swagger settings
builder.Services.AddOptions<SwaggerSettings>()
	.BindConfiguration(SwaggerSettings.SectionName)
	.ValidateDataAnnotations()
	.ValidateOnStart();

// Register and configure Swashbuckle
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();

// Register Dapper repositories
builder.Services.AddScoped<IUserRepository, SqlUserRepository>();
builder.Services.AddScoped<IRoutineRepository, SqlRoutineRepository>();
builder.Services.AddScoped<IExerciseRepository, SqlExerciseRepository>();
builder.Services.AddScoped<IWorkoutLogRepository, SqlWorkoutLogRepository>();

// Register application services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoutineService, RoutineService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IWorkoutLogService, WorkoutLogService>();

builder.Services.AddScoped<IRoutineExerciseService, RoutineExerciseService>();
builder.Services.AddScoped<IWorkoutExerciseService, WorkoutExerciseService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
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
