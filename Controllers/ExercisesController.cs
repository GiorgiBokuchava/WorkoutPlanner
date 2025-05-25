using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner_API.Domain.Entities;
using WorkoutPlanner_API.Application.Interfaces;

namespace WorkoutPlanner_API.Controllers;

/// <summary>
/// CRUD operations for Exercise resources
/// </summary>
[ApiController]
[Route("api/exercises")]
[Produces("application/json")]
public class ExercisesController : ControllerBase
{
	private readonly IExerciseRepository _exercises;

	public ExercisesController(IExerciseRepository exercises)
	{
		_exercises = exercises;
	}

	/// <summary>
	/// Returns all exercises
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Exercise>>> GetAll()
	{
		var exercises = await _exercises.GetAllExercisesAsync();
		return Ok(exercises);
	}

	[HttpGet("{id:int}")]
	public async Task<ActionResult<Exercise>> Get(int id)
	{
		var exercise = await _exercises.GetExerciseByIdAsync(id);
		if (exercise == null)
			return NotFound();
		return Ok(exercise);
	}

	[HttpPost]
	public async Task<ActionResult<Exercise>> Create(Exercise exercise)
	{
		await _exercises.AddExerciseAsync(exercise);
		return CreatedAtAction(nameof(Get), new { id = exercise.Id }, exercise);
	}

	[HttpPut("{id:int}")]
	public async Task<IActionResult> Update(int id, Exercise exercise)
	{
		exercise.Id = id;
		await _exercises.UpdateExerciseAsync(exercise);
		return NoContent();
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		await _exercises.DeleteExerciseAsync(id);
		return NoContent();
	}
}
