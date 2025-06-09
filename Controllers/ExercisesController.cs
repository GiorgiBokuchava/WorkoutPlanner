using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Common;
using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Controllers;
[ApiController]
[Route("api/exercises")]
[Produces("application/json")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
		var ex = await _service.GetExerciseByIdAsync(id);
		if (ex is null)
			return NotFound();
		return Ok(ex);
	}

	/// <summary>
	/// Creates a new exercise.
	/// </summary>
	/// <param name="request">The exercise creation request.</param>
	/// <returns>The created exercise DTO.</returns>
	[HttpPost]
	[Authorize(Roles = AppConstants.Roles.Admin)]
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
	[Authorize(Roles = AppConstants.Roles.Admin)]
	public async Task<IActionResult> Update(int id, UpdateExerciseRequest request)
	{
		var updated = await _service.UpdateExerciseAsync(id, request);
		if (!updated)
			return NotFound();
		return NoContent();
	}

	/// <summary>
	/// Deletes an exercise by its ID.
	/// </summary>
	/// <param name="id">The ID of the exercise to delete.</param>
	/// <returns>No content if successful; otherwise, NotFound.</returns>
	[HttpDelete("{id:int}")]
	[Authorize(Roles = AppConstants.Roles.Admin)]
	public async Task<IActionResult> Delete(int id)
	{
		var deleted = await _service.DeleteExerciseAsync(id);
		if (!deleted)
			return NotFound();
		return NoContent();
	}
}
