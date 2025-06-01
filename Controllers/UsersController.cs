using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner.Application.Services;
using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Controllers;
[ApiController]
[Route("api/users")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
	private readonly IUserService _service;

	public UsersController(IUserService service)
	{
		_service = service;
	}

	/// <summary>
	/// Retrieves all users.
	/// </summary>
	[HttpGet]
	public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
	{
		var list = await _service.GetAllUsersAsync();
		return Ok(list);
	}

	/// <summary>
	/// Retrieves a user by its ID.
	/// </summary>
	[HttpGet("{id:int}")]
	public async Task<ActionResult<UserDto>> Get(int id)
	{
		try
		{
			var dto = await _service.GetUserByIdAsync(id);
			return Ok(dto);
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
	}

	/// <summary>
	/// Creates a new user.
	/// </summary>
	[HttpPost]
	public async Task<ActionResult<UserDto>> Create(CreateUserRequest request)
	{
		var created = await _service.CreateUserAsync(request);
		return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
	}

	/// <summary>
	/// Updates an existing user.
	/// </summary>
	[HttpPut("{id:int}")]
	public async Task<IActionResult> Update(int id, CreateUserRequest request)
	{
		try
		{
			await _service.UpdateUserAsync(id, request);
			return NoContent();
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
	}

	/// <summary>
	/// Deletes a user by its ID.
	/// </summary>
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		try
		{
			await _service.DeleteUserAsync(id);
			return NoContent();
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
	}
}
