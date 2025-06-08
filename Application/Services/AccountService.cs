using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Contracts;
using WorkoutPlanner.Infrastructure.Repositories;
using WorkoutPlanner.Infrastructure.Security;

namespace WorkoutPlanner.Application.Services;

public class AccountService : IAccountService
{
	private readonly IUserService _userService;
	private readonly IPasswordService _passwordService;
	private readonly IUserRepository _userRepository;

	public AccountService(
			IUserService userService,
			IPasswordService passwordService,
			IUserRepository userRepository
		)
	{
		_userService = userService;
		_passwordService = passwordService;
		_userRepository = userRepository;
	}

	private static int? ExtractUserId(ClaimsPrincipal user)
	{
		var sub = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
			   ?? user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

		return int.TryParse(sub, out var id) ? id : null;
	}

	public async Task<UserDto?> GetCurrentUserAsync(ClaimsPrincipal user)
	{
		var id = ExtractUserId(user);
		return id is null ? null : await _userService.GetUserByIdAsync(id.Value);
	}

	public async Task<bool> UpdateCurrentUserAsync(ClaimsPrincipal user, UpdateUserRequest request)
	{
		var id = ExtractUserId(user);
		return id is not null && await _userService.UpdateUserAsync(id.Value, request);
	}

	public async Task<bool> DeleteCurrentUserAsync(ClaimsPrincipal user)
	{
		var id = ExtractUserId(user);
		return id is not null && await _userService.DeleteUserAsync(id.Value);
	}
}
