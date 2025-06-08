using WorkoutPlanner.Contracts;
using WorkoutPlanner.Domain.Entities;

namespace WorkoutPlanner.Application.Interfaces.Services;

public interface IUserService
{
	Task<IEnumerable<UserDto>> GetAllUsersAsync();
	Task<UserDto?> GetUserByIdAsync(int id);
	Task<User?> GetUserByEmailAsync(string email);
	Task<IEnumerable<string>> GetRolesByUserIdAsync(int userId);
	Task<UserDto> CreateUserAsync(CreateUserRequest request);
	Task<bool> UpdateUserAsync(int id, UpdateUserRequest request);
	Task<bool> DeleteUserAsync(int id);
}
