using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WorkoutPlanner_API.Infrastructure.Options;

public sealed class SwaggerSettings
{
    public const string SectionName = "Swagger";

    public string Version { get; init; } = default!;

    public string Title { get; init; } = default!;

    public string? Description { get; init; }
    public string? TermsOfService { get; init; }

    public ContactSettings Contact { get; init; } = new();
    public LicenseSettings License { get; init; } = new();

    public sealed record ContactSettings
    {
        public string? Name { get; init; }
        public string? Url { get; init; }
    }

    public sealed record LicenseSettings
    {
        public string? Name { get; init; }
        public string? Url { get; init; }
    }
}