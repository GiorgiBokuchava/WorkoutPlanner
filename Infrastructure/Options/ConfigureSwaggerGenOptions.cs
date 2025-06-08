using System.Reflection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WorkoutPlanner.Infrastructure.Options;

public sealed class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
	private readonly SwaggerSettings _settings;

	public ConfigureSwaggerGenOptions(IOptions<SwaggerSettings> opts) =>
		_settings = opts.Value;

	public void Configure(SwaggerGenOptions c)
	{
		c.SwaggerDoc(_settings.Version, new OpenApiInfo
		{
			Version = _settings.Version,
			Title = _settings.Title,
			Description = _settings.Description,
			TermsOfService = UriOrNull(_settings.TermsOfService),
			Contact = new OpenApiContact
			{
				Name = _settings.Contact.Name,
				Url = UriOrNull(_settings.Contact.Url)
			},
			License = new OpenApiLicense
			{
				Name = _settings.License.Name,
				Url = UriOrNull(_settings.License.Url)
			}
		});

		c.TagActionsBy(api => new[] { api.ActionDescriptor.RouteValues["controller"]! });
		c.DocInclusionPredicate((_, _) => true);

		// Include XML comments if available
		var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
		var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
		if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);

		// Add JWT auth support to Swagger
		c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
		{
			Description = "JWT Authorization header using the Bearer scheme (no 'Bearer ' prefix).",
			Name = "Authorization",
			In = ParameterLocation.Header,
			Type = SecuritySchemeType.Http,
			Scheme = "bearer",
			BearerFormat = "JWT"
		});

		c.AddSecurityRequirement(new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
					}
				},
				Array.Empty<string>()
			}
		});
	}

	private static Uri? UriOrNull(string? url) =>
		string.IsNullOrWhiteSpace(url) ? null : new Uri(url);
}