using System.ComponentModel.DataAnnotations;

namespace WorkoutPlanner.Infrastructure.Security;

public class JwtSettings
{
	[Required]
	public string Issuer { get; init; } = string.Empty;

	[Required]
	public string Audience { get; init; } = string.Empty;

	[Required, MinLength(32)]
	public string Key { get; init; } = string.Empty;

	[Range(1, 1440)]
	public int AccessTokenExpirationMinutes { get; init; } = 30;

	[Range(1, 365)]
	public int RefreshTokenExpirationDays { get; init; } = 7;
}