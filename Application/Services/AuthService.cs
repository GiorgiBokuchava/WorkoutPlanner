using MediatR;
using System.Security.Cryptography;
using System.Text;
using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Contracts;
using WorkoutPlanner.Infrastructure.Repositories;
using WorkoutPlanner.Infrastructure.Security;

namespace WorkoutPlanner.Application.Services;

public class AuthService : IAuthService
{
	private readonly IMediator _mediator;
	private readonly IUserService _userService;
	private readonly IPasswordService _passwordService;
	private readonly IJwtService _jwtService;
	private readonly IUserRepository _userRepository;

	public AuthService(
		IMediator mediator,
		IUserService userService,
		IPasswordService passwordService,
		IJwtService jwtService,
		IUserRepository userRepository
	)
	{
		_mediator = mediator;
		_userService = userService;
		_passwordService = passwordService;
		_jwtService = jwtService;
		_userRepository = userRepository;
	}

	public async Task<(string? AccessToken, string? RefreshToken)> LoginAsync(string email, string password)
	{
		var userEntity = await _userService.GetUserByEmailAsync(email);
		if (userEntity is null)
			return (null, null);

		bool isValid = _passwordService.VerifyPassword(password, userEntity.PasswordHash);
		if (!isValid)
			return (null, null);

		var roles = await _userService.GetRolesByUserIdAsync(userEntity.Id);
		var (accessToken, refreshToken) = await _jwtService.GenerateTokensAsync(userEntity.Id, roles);

		return (accessToken, refreshToken);
	}

	public async Task<(string? AccessToken, string? RefreshToken)> RefreshAsync(string rawRefreshToken)
	{
		byte[] rawBytes = Encoding.UTF8.GetBytes(rawRefreshToken);
		byte[] hashedBytes;
		using (var sha256 = SHA256.Create())
		{
			hashedBytes = sha256.ComputeHash(rawBytes);
		}
		string incomingHash = BitConverter.ToString(hashedBytes)
								  .Replace("-", "")
								  .ToLowerInvariant();

		var existingToken = await _userRepository.GetRefreshTokenByHashAsync(incomingHash);
		if (existingToken is null)
			return (null, null);

		if (existingToken.ExpiresAt < DateTime.UtcNow)
			return (null, null);

		if (existingToken.RevokedAt.HasValue)
			return (null, null);

		await _userRepository.RevokeRefreshTokenAsync(
			existingToken.Id,
			revokedAt: DateTime.UtcNow,
			revocationReason: 1
		);

		int userId = existingToken.UserId;
		var roles = await _userService.GetRolesByUserIdAsync(userId);
		var (newAccessToken, newRefreshToken) = await _jwtService.GenerateTokensAsync(userId, roles);

		return (newAccessToken, newRefreshToken);
	}

	public async Task LogoutAsync(int userId)
	{
		await _userRepository.RevokeAllRefreshTokensForUserAsync(userId);
	}
	public async Task<UserDto> RegisterAsync(CreateUserRequest request)
	{
		var dto = await _mediator.Send(
			new CreateUserCommand(request.Name, request.Email, request.Password));

		// ensure default “User” role exists
		var roles = await _userRepository.GetRolesForUserAsync(dto.Id);
		if (!roles.Contains("User"))
			await _userRepository.AssignRoleToUserAsync(dto.Id, "User");

		return dto;
	}
}
