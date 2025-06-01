using WorkoutPlanner.Contracts;
using WorkoutPlanner.Domain.Entities;
using Application.Interfaces;

namespace WorkoutPlanner.Application.Services;

public interface IUserService
{
	Task<IEnumerable<UserDto>> GetAllUsersAsync();
	Task<UserDto> GetUserByIdAsync(int id);
	Task<UserDto> CreateUserAsync(CreateUserRequest request);
	Task UpdateUserAsync(int id, CreateUserRequest request);
	Task DeleteUserAsync(int id);
}

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

	public async Task<UserDto> GetUserByIdAsync(int id)
	{
		var user = await _userRepository.GetUserByIdAsync(id);
		if (user is null) throw new KeyNotFoundException($"User with ID {id} not found.");

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
	public async Task UpdateUserAsync(int id, CreateUserRequest request)
	{
		var updated = new User
		{
			Id = id,
			Name = request.Name,
			Email = request.Email,
			PasswordHash = request.PasswordHash
		};

		await _userRepository.UpdateUserAsync(updated);
	}
	public async Task DeleteUserAsync(int id)
	{
		var existing = await _userRepository.GetUserByIdAsync(id);

		if (existing is null) throw new KeyNotFoundException($"User with ID {id} not found.");

		await _userRepository.DeleteUserAsync(id);
	}
}