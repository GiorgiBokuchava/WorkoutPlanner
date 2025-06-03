using WorkoutPlanner.Contracts;
using WorkoutPlanner.Domain.Entities;
using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Infrastructure.Repositories;

namespace WorkoutPlanner.Application.Services;

public class UserService : IUserService
{
	private readonly IUserRepository _userRepository;

	public UserService(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
	{
		var users = await _userRepository.GetAllUsersAsync();
		return users.Select(u => new UserDto(u.Id, u.Name, u.Email));
	}

	public async Task<UserDto?> GetUserByIdAsync(int id)
	{
		var user = await _userRepository.GetUserByIdAsync(id);
		if (user is null) return null;

		return new UserDto(user.Id, user.Name, user.Email);
	}

	public async Task<UserDto> CreateUserAsync(CreateUserRequest request)
	{
		var user = new User
		{
			Name = request.Name,
			Email = request.Email,
			PasswordHash = request.PasswordHash
		};

		var id = await _userRepository.AddUserAsync(user);
		user.Id = id;

		return new UserDto(user.Id, user.Name, user.Email);
	}

	public async Task<bool> UpdateUserAsync(int id, UpdateUserRequest request)
	{
		var existing = await _userRepository.GetUserByIdAsync(id);
		if (existing is null) return false;

		existing.Name = request.Name;
		existing.Email = request.Email;
		existing.PasswordHash = request.PasswordHash;

		await _userRepository.UpdateUserAsync(existing);
		return true;
	}

	public async Task<bool> DeleteUserAsync(int id)
	{
		var existing = await _userRepository.GetUserByIdAsync(id);

		if (existing is null) return false;

		await _userRepository.DeleteUserAsync(id);
		return true;
	}
}