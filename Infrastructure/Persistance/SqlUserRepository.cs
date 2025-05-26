using Application.Interfaces;
using WorkoutPlanner.Models;

namespace Infrastructure.Persistence;
public class SqlUserRepository : IUserRepository
{
	private readonly string _connectionString;

	public SqlUserRepository(IConfiguration configuration)
	{
		_connectionString = configuration.GetConnectionString("DefaultConnection")!;
	}

	public Task<User?> GetUserByIdAsync(int userId)
	{
		throw new NotImplementedException("Pending SQL implementation for HW5");
	}

	public Task<User?> GetUserByEmailAsync(string email)
	{
		throw new NotImplementedException("Pending SQL implementation for HW5");
	}

	public Task<IEnumerable<User>> GetAllUsersAsync()
	{
		throw new NotImplementedException("Pending SQL implementation for HW5");
	}

	public Task AddUserAsync(User user)
	{
		throw new NotImplementedException("Pending SQL implementation for HW5");
	}

	public Task UpdateUserAsync(User user)
	{
		throw new NotImplementedException("Pending SQL implementation for HW5");
	}

	public Task DeleteUserAsync(int userId)
	{
		throw new NotImplementedException("Pending SQL implementation for HW5");
	}
}