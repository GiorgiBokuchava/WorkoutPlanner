using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Application.Interfaces.Services;

public interface IAuthService
{
	Task<UserDto> RegisterAsync(CreateUserRequest request);
	Task<(string? AccessToken, string? RefreshToken)> LoginAsync(string email, string password);
	Task<(string? AccessToken, string? RefreshToken)> RefreshAsync(string rawRefreshToken);
	Task LogoutAsync(int userId);
}
