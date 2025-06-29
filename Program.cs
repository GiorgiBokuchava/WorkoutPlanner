using FluentValidation;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Application.Services;
using WorkoutPlanner.Application.Validators;
using WorkoutPlanner.Infrastructure.Options;
using WorkoutPlanner.Infrastructure.Persistence;
using WorkoutPlanner.Infrastructure.Repositories;
using WorkoutPlanner.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();

// Config binding
builder.Services.AddOptions<SwaggerSettings>()
	.BindConfiguration(SwaggerSettings.SectionName)
	.ValidateDataAnnotations()
	.ValidateOnStart();

builder.Services.AddOptions<JwtSettings>()
	.Bind(builder.Configuration.GetSection("JwtSettings"))
	.ValidateDataAnnotations()
	.ValidateOnStart();

builder.Services.AddSingleton(resolver =>
	resolver.GetRequiredService<IOptions<JwtSettings>>().Value
);

// Repositories
builder.Services.AddScoped<IUserRepository, SqlUserRepository>();
builder.Services.AddScoped<IRoutineRepository, SqlRoutineRepository>();
builder.Services.AddScoped<IExerciseRepository, SqlExerciseRepository>();
builder.Services.AddScoped<IWorkoutLogRepository, SqlWorkoutLogRepository>();

builder.Services
	.AddHealthChecks()
	.AddSqlServer(
		// null-coalesce to satisfy the compiler
		builder.Configuration.GetConnectionString("DefaultConnection")
			?? throw new InvalidOperationException("DefaultConnection is not set"),
		name: "sql",
		tags: new[] { "db", "sql" },
		timeout: TimeSpan.FromSeconds(5)
	);

builder.Services.AddHealthChecksUI(setup =>
{
	setup.AddHealthCheckEndpoint("WorkoutPlanner", "/health");
}).AddInMemoryStorage();

// Application services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoutineService, RoutineService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IWorkoutLogService, WorkoutLogService>();
builder.Services.AddScoped<IRoutineExerciseService, RoutineExerciseService>();
builder.Services.AddScoped<IWorkoutExerciseService, WorkoutExerciseService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IPasswordService, BcryptPasswordService>();
builder.Services.AddScoped<IAccountService, AccountService>();

// MediatR & AutoMapper
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();

// JWT Auth
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var keyBytes = Encoding.UTF8.GetBytes(jwtSettings!.Key);

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opts =>
{
	opts.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = jwtSettings.Issuer,
		ValidAudience = jwtSettings.Audience,
		IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
		ClockSkew = TimeSpan.FromSeconds(30)
	};
	opts.Events = new JwtBearerEvents
	{
		OnAuthenticationFailed = ctx =>
		{
			Console.WriteLine($"[JWT ERROR] {ctx.Exception.GetType().Name}: {ctx.Exception.Message}");
			return Task.CompletedTask;
		}
	};
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	var swagger = app.Services.GetRequiredService<IOptions<SwaggerSettings>>().Value;
	app.UseSwagger();
	app.UseSwaggerUI(ui =>
	{
		ui.SwaggerEndpoint($"/swagger/{swagger.Version}/swagger.json", $"{swagger.Title} {swagger.Version}");
	});
}

// expose the raw health check
app.MapHealthChecks("/health", new HealthCheckOptions
{
	ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// expose the dashboard UI
app.MapHealthChecksUI(options =>
{
	options.UIPath = "/health-ui";
	options.ApiPath = "/health-ui-api";
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
try
{
	logger.LogInformation("Application starting up");
	app.Run();
	logger.LogInformation("Application stopped");
}
catch (Exception e)
{
	logger.LogCritical(e, "Application failed to start");
	throw;
}