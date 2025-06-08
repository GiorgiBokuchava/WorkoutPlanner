using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Contracts;
using WorkoutPlanner.Domain.Entities;
using WorkoutPlanner.Infrastructure.Repositories;
using WorkoutPlanner.Infrastructure.Security;

namespace WorkoutPlanner.Application.Services;

public class UserService : IUserService
{
	private readonly IUserRepository _repository;
	private readonly IPasswordService _passwordService;

	public UserService(IUserRepository repository, IPasswordService passwordService)
	{
		_repository = repository;
		_passwordService = passwordService;
	}

	public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
	{
		var users = await _repository.GetAllUsersAsync();
		return users.Select(u => new UserDto(u.Id, u.Name, u.Email));
	}

	public async Task<UserDto?> GetUserByIdAsync(int id)
	{
		var user = await _repository.GetUserByIdAsync(id);
		if (user is null) return null;

		return new UserDto(user.Id, user.Name, user.Email);
	}

	public Task<User?> GetUserByEmailAsync(string email)
	{
		return _repository.GetUserByEmailAsync(email);
	}

	public Task<IEnumerable<string>> GetRolesByUserIdAsync(int userId)
	{
		return _repository.GetRolesForUserAsync(userId);
	}

	public async Task<UserDto> CreateUserAsync(CreateUserRequest request)
	{
		var user = new User
		{
			Name = request.Name,
			Email = request.Email,
			PasswordHash = _passwordService.HashPassword(request.Password)
		};

		var id = await _repository.AddUserAsync(user);
		return new UserDto(id, user.Name, user.Email);
	}

	public async Task<bool> UpdateUserAsync(int id, UpdateUserRequest request)
	{
		var user = await _repository.GetUserByIdAsync(id);
		if (user == null) return false;

		user.Name = request.Name;
		user.Email = request.Email;
		if (!string.IsNullOrEmpty(request.Password))
		{
			user.PasswordHash = _passwordService.HashPassword(request.Password);
		}

		await _repository.UpdateUserAsync(user);
		return true;
	}

	public async Task<bool> DeleteUserAsync(int id)
	{
		var existing = await _repository.GetUserByIdAsync(id);

		if (existing is null) return false;

		await _repository.DeleteUserAsync(id);
		return true;
	}
}