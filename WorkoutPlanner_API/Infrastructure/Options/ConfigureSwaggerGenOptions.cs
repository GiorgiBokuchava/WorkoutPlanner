using System.Reflection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WorkoutPlanner_API.Infrastructure.Options;

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

        // include XML comments if they exist
        var xml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var path = Path.Combine(AppContext.BaseDirectory, xml);
        if (File.Exists(path)) c.IncludeXmlComments(path);
    }

    private static Uri? UriOrNull(string? url) =>
        string.IsNullOrWhiteSpace(url) ? null : new Uri(url);
}