using WorkoutPlanner.Domain.Entities;

namespace WorkoutPlanner.Infrastructure.Repositories;
public interface IUserRepository
{
	Task<IEnumerable<User>> GetAllUsersAsync();
	Task<User?> GetUserByIdAsync(int userId);
	Task<User?> GetUserByEmailAsync(string email);
	Task<int> AddUserAsync(User user);
	Task UpdateUserAsync(User user);
	Task DeleteUserAsync(int userId);

	Task<int> AddRefreshTokenAsync(int userId, string tokenHash, DateTime expiresAt);
	Task<RefreshToken?> GetRefreshTokenByHashAsync(string tokenHash);
	Task RevokeRefreshTokenAsync(int refreshTokenId, DateTime revokedAt, byte revocationReason);
	Task RevokeAllRefreshTokensForUserAsync(int userId);

	Task<IEnumerable<string>> GetRolesForUserAsync(int userId);
	Task AssignRoleToUserAsync(int userId, string roleName);
}