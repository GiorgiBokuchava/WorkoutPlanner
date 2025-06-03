using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Application.Services;
using WorkoutPlanner.Application.Validators;
using WorkoutPlanner.Infrastructure.Options;
using WorkoutPlanner.Infrastructure.Persistance;
using WorkoutPlanner.Infrastructure.Persistence;
using WorkoutPlanner.Infrastructure.Repositories;

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

// Register application services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoutineService, RoutineService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IWorkoutLogService, WorkoutLogService>();

builder.Services.AddScoped<IRoutineExerciseService, RoutineExerciseService>();
builder.Services.AddScoped<IWorkoutExerciseService, WorkoutExerciseService>();

builder.Services.AddMediatR(cfg =>
{
	cfg.RegisterServicesFromAssemblyContaining<Program>();
});

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();

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