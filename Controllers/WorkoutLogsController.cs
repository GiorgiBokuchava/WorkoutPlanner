using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner.Application.Interfaces;
using WorkoutPlanner.Application.Services;
using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Controllers;

[ApiController]
[Route("api/workoutLogs")]
[Produces("application/json")]
public class WorkoutLogsController : ControllerBase
{
	private readonly IWorkoutLogService _service;

	public WorkoutLogsController(IWorkoutLogService service)
	{
		_service = service;
	}

	/// <summary>
	/// Gets all workout logs.
	/// </summary>
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<IEnumerable<WorkoutLogDto>>> GetAll()
	{
		var list = await _service.GetAllWorkoutLogsAsync();
		return Ok(list);
	}

	/// <summary>
	/// Gets a workout log by its ID.
	/// </summary>
	/// <param name="id">Workout log ID</param>
	[HttpGet("{id:int}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<WorkoutLogDto>> Get(int id)
	{
		try
		{
			var dto = await _service.GetWorkoutLogByIdAsync(id);
			return Ok(dto);
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
	}

	/// <summary>
	/// Gets all logs for a specific user.
	/// </summary>
	/// <param name="userId">User ID</param>
	[HttpGet("users/{userId:int}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<IEnumerable<WorkoutLogDto>>> GetByUser(int userId)
	{
		var list = await _service.GetWorkoutLogsByUserIdAsync(userId);
		return Ok(list);
	}

	/// <summary>
	/// Creates a new workout log.
	/// </summary>
	/// <param name="userId">User ID</param>
	/// <param name="request">Workout log data</param>
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public async Task<ActionResult<WorkoutLogDto>> Create(
		[FromQuery] int userId,
		[FromBody] CreateWorkoutLogRequest request)
	{
		var created = await _service.CreateWorkoutLogAsync(userId, request);
		return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
	}

	/// <summary>
	/// Updates an existing workout log.
	/// </summary>
	/// <param name="id">Workout log ID</param>
	/// <param name="userId">User ID</param>
	/// <param name="request">Updated data</param>
	[HttpPut("{id:int}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Update(
		int id,
		[FromQuery] int userId,
		[FromBody] CreateWorkoutLogRequest request)
	{
		try
		{
			await _service.UpdateWorkoutLogAsync(id, request);
			return NoContent();
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
	}

	/// <summary>
	/// Deletes a workout log by its ID.
	/// </summary>
	/// <param name="id">Workout log ID</param>
	[HttpDelete("{id:int}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Delete(int id)
	{
		try
		{
			await _service.DeleteWorkoutLogAsync(id);
			return NoContent();
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
	}
}
