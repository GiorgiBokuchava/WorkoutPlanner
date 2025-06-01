using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner.Application.Services;
using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Controllers;
[ApiController]
[Route("api/routines")]
[Produces("application/json")]
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
		try
		{
			var dto = await _service.GetRoutineByIdAsync(id);
			return Ok(dto);
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
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
	public async Task<ActionResult<RoutineDto>> Create(CreateRoutineRequest request)
	{
		var created = await _service.CreateRoutineAsync(request);
		return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
	}

	/// <summary>
	/// Updates an existing routine.
	/// </summary>
	[HttpPut("{id:int}")]
	public async Task<IActionResult> Update(int id, CreateRoutineRequest request)
	{
		try
		{
			await _service.UpdateRoutineAsync(id, request);
			return NoContent();
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
	}

	/// <summary>
	/// Deletes a routine by its ID.
	/// </summary>
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		try
		{
			await _service.DeleteRoutineAsync(id);
			return NoContent();
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
	}
}
