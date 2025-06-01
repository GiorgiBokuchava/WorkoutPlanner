using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner.Contracts;
using WorkoutPlanner.Application.Services;

namespace WorkoutPlanner.Controllers;
[ApiController]
[Route("api/workoutExercises")]
[Produces("application/json")]
public class WorkoutExercisesController : ControllerBase
{
	private readonly IWorkoutExerciseService _service;

	public WorkoutExercisesController(IWorkoutExerciseService service)
	{
		_service = service;
	}

	/// <summary>
	/// Retrieves all workout exercises.
	/// </summary>
	[HttpGet]
	public async Task<ActionResult<IEnumerable<WorkoutExerciseDto>>> GetAll()
	{
		var list = await _service.GetAllWorkoutExercisesAsync();
		return Ok(list);
	}

	/// <summary>
	/// Retrieves a single workout exercise by its ID.
	/// </summary>
	[HttpGet("{id:int}")]
	public async Task<ActionResult<WorkoutExerciseDto>> Get(int id)
	{
		try
		{
			var dto = await _service.GetWorkoutExerciseByIdAsync(id);
			return Ok(dto);
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
	}

	/// <summary>
	/// Creates a new workout exercise entry.
	/// </summary>
	[HttpPost]
	public async Task<ActionResult<WorkoutExerciseDto>> Create(CreateWorkoutExerciseRequest request)
	{
		var created = await _service.CreateWorkoutExerciseAsync(request);
		return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
	}

	/// <summary>
	/// Updates an existing workout exercise.
	/// </summary>
	[HttpPut("{id:int}")]
	public async Task<IActionResult> Update(int id, CreateWorkoutExerciseRequest request)
	{
		try
		{
			await _service.UpdateWorkoutExerciseAsync(id, request);
			return NoContent();
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
	}

	/// <summary>
	/// Deletes a workout exercise by its ID.
	/// </summary>
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		try
		{
			await _service.DeleteWorkoutExerciseAsync(id);
			return NoContent();
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
	}
}