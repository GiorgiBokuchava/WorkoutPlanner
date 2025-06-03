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
}
