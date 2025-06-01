using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner.Application.Services;
using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Controllers;
[ApiController]
[Route("api/exercises")]
[Produces("application/json")]
public class ExercisesController : ControllerBase
{
	private readonly IExerciseService _service;

	public ExercisesController(IExerciseService service)
	{
		_service = service;
	}

	/// <summary>
	/// Gets all exercises.
	/// </summary>
	/// <returns>A list of exercise DTOs.</returns>
	[HttpGet]
	public async Task<ActionResult<IEnumerable<ExerciseDto>>> GetAll()
	{
		var list = await _service.GetAllExercisesAsync();
		return Ok(list);
	}

	/// <summary>
	/// Gets a specific exercise by its ID.
	/// </summary>
	/// <param name="id">The ID of the exercise.</param>
	/// <returns>The exercise DTO if found; otherwise, NotFound.</returns>
	[HttpGet("{id:int}")]
	public async Task<ActionResult<ExerciseDto>> Get(int id)
	{
		try
		{
			var ex = await _service.GetExerciseByIdAsync(id);
			return Ok(ex);
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
	}

	/// <summary>
	/// Creates a new exercise.
	/// </summary>
	/// <param name="request">The exercise creation request.</param>
	/// <returns>The created exercise DTO.</returns>
	[HttpPost]
	public async Task<ActionResult<ExerciseDto>> Create(CreateExerciseRequest request)
	{
		var created = await _service.CreateExerciseAsync(request);
		return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
	}

	/// <summary>
	/// Updates an existing exercise.
	/// </summary>
	/// <param name="id">The ID of the exercise to update.</param>
	/// <param name="request">The updated exercise data.</param>
	/// <returns>No content if successful; otherwise, NotFound.</returns>
	[HttpPut("{id:int}")]
	public async Task<IActionResult> Update(int id, CreateExerciseRequest request)
	{
		try
		{
			await _service.UpdateExerciseAsync(id, request);
			return NoContent();
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
	}

	/// <summary>
	/// Deletes an exercise by its ID.
	/// </summary>
	/// <param name="id">The ID of the exercise to delete.</param>
	/// <returns>No content if successful; otherwise, NotFound.</returns>
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		try
		{
			await _service.DeleteExerciseAsync(id);
			return NoContent();
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
	}
}
