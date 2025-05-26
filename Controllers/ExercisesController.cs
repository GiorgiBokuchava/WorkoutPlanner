using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner.Models;
using static WorkoutPlanner.Data.DataStore;

namespace WorkoutPlanner.Controllers;

/// <summary>
/// CRUD operations for Exercise resources
/// </summary>
[ApiController]
[Route("api/exercises")]
[Produces("application/json")]
public class ExercisesController : ControllerBase
{
	/// <summary>
	/// Returns all exercises
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	public ActionResult<IEnumerable<Exercise>> GetAll()
	{
		return Ok(Exercises);
	}

	[HttpGet("{id:int}")]
	public ActionResult<Exercise> Get(int id)
	{
		var exercise = Exercises.FirstOrDefault(e => e.Id == id);
		if (exercise != null)
		{
			return Ok(exercise);
		}

		return NotFound();
	}

	[HttpPost]
	public ActionResult<Exercise> Create(Exercise exercise)
	{
		exercise.Id = Exercises.Count + 1;
		Exercises.Add(exercise);
		return CreatedAtAction(nameof(Get), new { id = exercise.Id }, exercise);
	}

	[HttpPut("{id:int}")]
	public IActionResult Update(int id, Exercise exercise)
	{
		var initial = Exercises.FirstOrDefault(e => e.Id == id);
		if (initial == null)
		{
			return NotFound();
		}
		initial.Name = exercise.Name;
		initial.Target = exercise.Target;
		initial.Equipment = exercise.Equipment;

		return NoContent();
	}

	[HttpDelete("{id:int}")]
	public IActionResult Delete(int id)
	{
		var exercise = Exercises.FirstOrDefault(e => e.Id == id);
		if (exercise == null)
		{
			return NotFound();
		}
		Exercises.Remove(exercise);

		return NoContent();
	}
}
