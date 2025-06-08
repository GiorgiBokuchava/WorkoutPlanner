using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;
	private readonly IMediator _mediator;

	public AuthController(IAuthService authService, IMediator mediator)
	{
		_authService = authService;
		_mediator = mediator;
	}

	/// <summary>
	/// Registers a new user.
	/// </summary>
	/// <param name="command">The registration command with name, email, and password.</param>
	/// <returns>The created user DTO.</returns>
	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] CreateUserCommand command)
	{
		var userDto = await _mediator.Send(command);
		return CreatedAtAction(nameof(Register), new { id = userDto.Id }, userDto);
	}

	/// <summary>
	/// Authenticates a user and returns JWT tokens.
	/// </summary>
	/// <param name="request">The login request with email and password.</param>
	/// <returns>Access token and refresh token if valid; otherwise Unauthorized.</returns>
	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginRequest request)
	{
		var (accessToken, refreshToken) = await _authService.LoginAsync(request.Email, request.Password);

		if (accessToken is null || refreshToken is null)
			return Unauthorized(new { message = "Invalid email or password" });

		return Ok(new
		{
			AccessToken = accessToken,
			RefreshToken = refreshToken
		});
	}

	/// <summary>
	/// Refreshes JWT tokens using a valid refresh token.
	/// </summary>
	/// <param name="request">The refresh token request.</param>
	/// <returns>New access and refresh tokens if valid; otherwise Unauthorized.</returns>
	[HttpPost("refresh")]
	public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
	{
		var (newAccess, newRefresh) = await _authService.RefreshAsync(request.RefreshToken);

		if (newAccess is null || newRefresh is null)
			return Unauthorized(new { message = "Invalid or expired refresh token." });

		return Ok(new
		{
			AccessToken = newAccess,
			RefreshToken = newRefresh
		});
	}

	/// <summary>
	/// Logs out the currently authenticated user and revokes all their refresh tokens.
	/// </summary>
	/// <returns>No content if successful; otherwise Unauthorized.</returns>
	[HttpPost("logout")]
	[Authorize]
	public async Task<IActionResult> Logout()
	{
		var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
					   ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

		if (!int.TryParse(userIdClaim, out var userId))
			return Unauthorized();

		await _authService.LogoutAsync(userId);
		return NoContent();
	}
}
