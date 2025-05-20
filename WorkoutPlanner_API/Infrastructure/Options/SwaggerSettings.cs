using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace WorkoutPlanner_API.Infrastructure.Options;

public sealed class SwaggerSettings
{
    public const string SectionName = "Swagger";

    [Required]
    public string Version { get; init; } = default!;

    [Required]
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