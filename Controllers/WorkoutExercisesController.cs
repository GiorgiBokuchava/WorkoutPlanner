using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Controllers;
[ApiController]
[Route("api/workoutExercises")]
[Produces("application/json")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
		var dto = await _service.GetWorkoutExerciseByIdAsync(id);
		if (dto is null)
			return NotFound();
		return Ok(dto);
	}

	/// <summary>
	/// Creates a new workout exercise entry.
	/// </summary>
	[HttpPost]
	[Authorize(Roles = "Admin")]
	public async Task<ActionResult<WorkoutExerciseDto>> Create(CreateWorkoutExerciseRequest request)
	{
		var created = await _service.CreateWorkoutExerciseAsync(request);
		return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
	}

	/// <summary>
	/// Updates an existing workout exercise.
	/// </summary>
	[HttpPut("{id:int}")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Update(int id, UpdateWorkoutExerciseRequest request)
	{
		var updated = await _service.UpdateWorkoutExerciseAsync(id, request);
		if (!updated)
			return NotFound();
		return NoContent();
	}

	/// <summary>
	/// Deletes a workout exercise by its ID.
	/// </summary>
	[HttpDelete("{id:int}")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Delete(int id)
	{
		var deleted = await _service.DeleteWorkoutExerciseAsync(id);
		if (!deleted)
			return NotFound();
		return NoContent();
	}
}