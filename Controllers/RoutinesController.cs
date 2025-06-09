using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Common;
using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Controllers;
[ApiController]
[Route("api/routines")]
[Produces("application/json")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RoutinesController : ControllerBase
{
	private readonly IRoutineService _service;

	public RoutinesController(IRoutineService service)
	{
		_service = service;
	}

	/// <summary>
	/// Retrieves all routines.
	/// </summary>
	[HttpGet]
	public async Task<ActionResult<IEnumerable<RoutineDto>>> GetAll()
	{
		var list = await _service.GetAllRoutinesAsync();
		return Ok(list);
	}

	/// <summary>
	/// Retrieves a routine by its ID.
	/// </summary>
	[HttpGet("{id:int}")]
	public async Task<ActionResult<RoutineDto>> Get(int id)
	{
		var dto = await _service.GetRoutineByIdAsync(id);
		if (dto is null)
			return NotFound();
		return Ok(dto);
	}

	/// <summary>
	/// Retrieves all routines for a specific user.
	/// </summary>
	[HttpGet("users/{userId:int}")]
	public async Task<ActionResult<IEnumerable<RoutineDto>>> GetByUser(int userId)
	{
		var list = await _service.GetRoutineByUserIdAsync(userId);
		return Ok(list);
	}

	/// <summary>
	/// Creates a new routine.
	/// </summary>
	[HttpPost]
	[Authorize(Roles = AppConstants.Roles.Admin)]
	public async Task<ActionResult<RoutineDto>> Create(CreateRoutineRequest request)
	{
		var created = await _service.CreateRoutineAsync(request);
		return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
	}

	/// <summary>
	/// Updates an existing routine.
	/// </summary>
	[HttpPut("{id:int}")]
	[Authorize(Roles = AppConstants.Roles.Admin)]
	public async Task<IActionResult> Update(int id, UpdateRoutineRequest request)
	{
		var updated = await _service.UpdateRoutineAsync(id, request);
		if (!updated)
			return NotFound();
		return NoContent();
	}

	/// <summary>
	/// Deletes a routine by its ID.
	/// </summary>
	[HttpDelete("{id:int}")]
	[Authorize(Roles = AppConstants.Roles.Admin)]
	public async Task<IActionResult> Delete(int id)
	{
		var deleted = await _service.DeleteRoutineAsync(id);
		if (!deleted)
			return NotFound();
		return NoContent();
	}
}
