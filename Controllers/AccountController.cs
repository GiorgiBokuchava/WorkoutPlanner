using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Controllers;

[ApiController]
[Route("api/account")]
[Produces("application/json")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AccountController : ControllerBase
{
	private readonly IAccountService _accountService;

	public AccountController(IAccountService accountService)
	{
		_accountService = accountService;
	}

	/// <summary>
	/// Gets the profile of the currently authenticated user.
	/// </summary>
	/// <returns>The user's public profile information.</returns>
	[HttpGet("me")]
	public async Task<ActionResult<UserDto>> GetMe()
	{
		var user = await _accountService.GetCurrentUserAsync(User);
		if (user is null) return Unauthorized();
		return Ok(user);
	}

	/// <summary>
	/// Updates the profile of the currently authenticated user.
	/// </summary>
	/// <param name="request">Updated name, email, or password.</param>
	/// <returns>NoContent if updated, NotFound if user is missing.</returns>
	[HttpPut("me")]
	public async Task<IActionResult> UpdateMe([FromBody] UpdateUserRequest request)
	{
		var updated = await _accountService.UpdateCurrentUserAsync(User, request);
		return updated ? NoContent() : NotFound();
	}

	/// <summary>
	/// Deletes the currently authenticated user's account.
	/// </summary>
	/// <returns>NoContent if deleted, NotFound if user is missing.</returns>
	[HttpDelete("me")]
	public async Task<IActionResult> DeleteMe()
	{
		var deleted = await _accountService.DeleteCurrentUserAsync(User);
		return deleted ? NoContent() : NotFound();
	}
}
