namespace WorkoutPlanner.Infrastructure.Security;

public interface IJwtService
{
	Task<(string AccessToken, string? RefreshToken)> GenerateTokensAsync(int userId,
		IEnumerable<string> roles);
}