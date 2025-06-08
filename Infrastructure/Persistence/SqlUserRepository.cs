using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WorkoutPlanner.Domain.Entities;
using WorkoutPlanner.Infrastructure.Repositories;

namespace WorkoutPlanner.Infrastructure.Persistence;

public class SqlUserRepository : IUserRepository
{
	private readonly IConfiguration _configuration;

	public SqlUserRepository(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	private IDbConnection GetConnection()
	{
		return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"))
			?? throw new InvalidOperationException("DefaultConnection is not set");
	}

	public async Task<IEnumerable<User>> GetAllUsersAsync()
	{
		const string sql = @"
                SELECT
                    id             AS Id,
                    name           AS Name,
                    email          AS Email,
                    password_hash  AS PasswordHash
                FROM [Identity].Users;
            ";
		using var db = GetConnection();
		return await db.QueryAsync<User>(sql);
	}

	public async Task<User?> GetUserByIdAsync(int userId)
	{
		const string sql = @"
                SELECT
                    id             AS Id,
                    name           AS Name,
                    email          AS Email,
                    password_hash  AS PasswordHash
                FROM [Identity].Users
                WHERE id = @Id;
            ";
		using var db = GetConnection();
		return await db.QuerySingleOrDefaultAsync<User>(sql, new { Id = userId });
	}

	public async Task<User?> GetUserByEmailAsync(string email)
	{
		const string sql = @"
                SELECT
                    id             AS Id,
                    name           AS Name,
                    email          AS Email,
                    password_hash  AS PasswordHash
                FROM [Identity].Users
                WHERE email = @Email;
            ";
		using var db = GetConnection();
		return await db.QuerySingleOrDefaultAsync<User>(sql, new { Email = email });
	}

	public async Task<int> AddUserAsync(User user)
	{
		const string sql = @"
                INSERT INTO [Identity].Users (name, email, password_hash)
                OUTPUT INSERTED.id
                VALUES (@Name, @Email, @PasswordHash);
            ";
		using var db = GetConnection();
		return await db.ExecuteScalarAsync<int>(sql, user);
	}

	public async Task UpdateUserAsync(User user)
	{
		const string sql = @"
                UPDATE [Identity].Users
                   SET name          = @Name,
                       email         = @Email,
                       password_hash = @PasswordHash
                 WHERE id = @Id;
            ";
		using var db = GetConnection();
		await db.ExecuteAsync(sql, user);
	}

	public async Task DeleteUserAsync(int userId)
	{
		const string sql = @"
                DELETE FROM [Identity].Users
                WHERE id = @Id;
            ";
		using var db = GetConnection();
		await db.ExecuteAsync(sql, new { Id = userId });
	}

	public async Task<int> AddRefreshTokenAsync(int userId, string tokenHash, DateTime expiresAt)
	{
		const string sql = @"
                INSERT INTO [Identity].RefreshTokens (user_id, token_hash, expires_at)
                OUTPUT INSERTED.id
                VALUES (@UserId, @TokenHash, @ExpiresAt);
            ";
		using var db = GetConnection();
		return await db.ExecuteScalarAsync<int>(sql, new
		{
			UserId = userId,
			TokenHash = tokenHash,
			ExpiresAt = expiresAt
		});
	}

	public async Task<RefreshToken?> GetRefreshTokenByHashAsync(string tokenHash)
	{
		const string sql = @"
				SELECT
					id AS Id,
					user_id AS UserId,
					token_hash AS TokenHash,
					expires_at AS ExpiresAt,
					revoked_at AS RevokedAt,
					revocation_reason AS RevocationReason
				FROM [Identity].RefreshTokens
				WHERE token_hash = @TokenHash;";

		using var db = GetConnection();
		return await db.QuerySingleOrDefaultAsync<RefreshToken>(sql, new { TokenHash = tokenHash });
	}

	public async Task RevokeRefreshTokenAsync(int refreshTokenId, DateTime revokedAt, byte revocationReason)
	{
		const string sql = @"
				UPDATE [Identity].RefreshTokens
				SET revoked_at = @RevokedAt,
					revocation_reason = @RevocationReason
				WHERE id = @Id;";

		using var db = GetConnection();
		await db.ExecuteAsync(sql, new
		{
			Id = refreshTokenId,
			RevokedAt = revokedAt,
			RevocationReason = revocationReason
		});
	}

	public async Task RevokeAllRefreshTokensForUserAsync(int userId)
	{
		const string sql = @"
                UPDATE [Identity].RefreshTokens
                   SET revoked_at = @Now,
                       revocation_reason = 0
                 WHERE user_id = @UserId AND revoked_at IS NULL;";

		using var db = GetConnection();
		await db.ExecuteAsync(sql, new
		{
			UserId = userId,
			Now = DateTime.UtcNow
		});
	}

	public async Task<IEnumerable<string>> GetRolesForUserAsync(int userId)
	{
		const string sql = @"
                SELECT r.name
                FROM [Identity].Roles r
                JOIN [Identity].UserRoles ur
                  ON r.id = ur.role_id
                WHERE ur.user_id = @UserId;";

		using var db = GetConnection();
		return await db.QueryAsync<string>(sql, new { UserId = userId });
	}

	public async Task AssignRoleToUserAsync(int userId, string roleName)
	{
		const string sql = @"
        INSERT INTO [Identity].UserRoles (user_id, role_id)
        SELECT @UserId, r.id
        FROM [Identity].Roles r
        WHERE r.name = @RoleName
			AND NOT EXISTS (
              SELECT 1 FROM [Identity].UserRoles ur
              WHERE ur.user_id = @UserId AND ur.role_id = r.id
          );";

		using var db = GetConnection();
		await db.ExecuteAsync(sql, new { UserId = userId, RoleName = roleName });
	}
}
