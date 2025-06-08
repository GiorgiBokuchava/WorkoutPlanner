using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Controllers;
[ApiController]
[Route("api/users")]
[Produces("application/json")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
public class UsersController : ControllerBase
{
	private readonly IUserService _service;
	private readonly IMediator _mediator;

	public UsersController(IUserService service, IMediator mediator)
	{
		_service = service;
		_mediator = mediator;
	}

	/// <summary>
	/// Retrieves all users.
	/// </summary>
	[HttpGet]
	public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
	{
		return Ok(await _mediator.Send(new GetAllUsersQuery()));
	}

	/// <summary>
	/// Retrieves a user by its ID.
	/// </summary>
	[HttpGet("{id:int}")]
	public async Task<ActionResult<UserDto>> Get(int id)
	{
		var dto = await _mediator.Send(new GetUserByIdQuery(id));
		if (dto is null)
			return NotFound();
		return Ok(dto);
	}

	/// <summary>
	/// Creates a new user.
	/// </summary>
	[HttpPost]
	public async Task<ActionResult<UserDto>> Create(CreateUserCommand cmd)
	{
		var created = await _mediator.Send(cmd);
		return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
	}

	/// <summary>
	/// Updates an existing user.
	/// </summary>
	[HttpPut("{id:int}")]
	public async Task<IActionResult> Update(int id, UpdateUserCommand cmd)
	{
		if (id != cmd.Id) return BadRequest();

		var result = await _mediator.Send(cmd);
		if (result.Equals(Unit.Value))
			return NoContent();

		return NotFound();
	}

	/// <summary>
	/// Deletes a user by its ID.
	/// </summary>
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var deleted = await _mediator.Send(new DeleteUserCommand(id));
		if (!deleted.Equals(Unit.Value))
			return NotFound();

		return NoContent();
	}
}
