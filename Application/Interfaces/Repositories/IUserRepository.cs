using WorkoutPlanner.Domain.Entities;

namespace WorkoutPlanner.Application.Interfaces.Repositories;
public interface IUserRepository
{
	Task<IEnumerable<User>> GetAllUsersAsync();
	Task<User?> GetUserByIdAsync(int userId);
	Task<User?> GetUserByEmailAsync(string email);
	Task<int> AddUserAsync(User user);
	Task UpdateUserAsync(User user);
	Task DeleteUserAsync(int userId);
}