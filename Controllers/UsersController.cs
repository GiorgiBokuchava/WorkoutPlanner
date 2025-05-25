using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner_API.Domain.Entities;
using Application.Interfaces;

namespace WorkoutPlanner_API.Controllers;

/// <summary>
/// Basic CRUD operations for User resources
/// </summary>
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
	private readonly IUserRepository _users;

	public UsersController(IUserRepository users)
	{
		_users = users;
	}

	[HttpGet]
	public async Task<IEnumerable<User>> GetAll()
	{
		return await _users.GetAllUsersAsync();
	}

	[HttpGet("{id:int}")]
	public async Task<ActionResult<User>> Get(int id)
	{
		var user = await _users.GetUserByIdAsync(id);
		if (user is null)
			return NotFound();
		return Ok(user);
	}

	/// <summary>
	/// Creates a new user
	/// </summary>
	/// <remarks>
	/// The email must be unique but this demo does not validate it.
	/// </remarks>
	[HttpPost]
	public async Task<ActionResult<User>> Create(User user)
	{
		await _users.AddUserAsync(user);
		// Assuming AddUserAsync sets user.Id or it is retrieved any other way
		return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
	}

	[HttpPut("{id:int}")]
	public async Task<IActionResult> Update(int id, User user)
	{
		user.Id = id;
		await _users.UpdateUserAsync(user);
		return NoContent();
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		await _users.DeleteUserAsync(id);
		return NoContent();
	}
}
