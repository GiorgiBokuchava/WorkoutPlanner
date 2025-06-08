using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Application.Services;
using WorkoutPlanner.Application.Validators;
using WorkoutPlanner.Infrastructure.Options;
using WorkoutPlanner.Infrastructure.Persistance;
using WorkoutPlanner.Infrastructure.Persistence;
using WorkoutPlanner.Infrastructure.Repositories;
using WorkoutPlanner.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

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
var keyBytes = Encoding.UTF8.GetBytes(jwtSettings.Key);

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

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();