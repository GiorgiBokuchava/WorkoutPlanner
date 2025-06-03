using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner.Application.Interfaces.Services;
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
		var dto = await _service.GetUserByIdAsync(id);
		if (dto is null)
			return NotFound();
		return Ok(dto);
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
	public async Task<IActionResult> Update(int id, UpdateUserRequest request)
	{
		var updated = await _service.UpdateUserAsync(id, request);
		if (!updated)
			return NotFound();
		return NoContent();
	}

	/// <summary>
	/// Deletes a user by its ID.
	/// </summary>
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var deleted = await _service.DeleteUserAsync(id);
		if (!deleted)
			return NotFound();
		return NoContent();
	}
}
