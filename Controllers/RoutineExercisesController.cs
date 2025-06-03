using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Controllers;
[ApiController]
[Route("api/routineExercises")]
[Produces("application/json")]
public class RoutineExercisesController : ControllerBase
{
	private readonly IRoutineExerciseService _service;

	public RoutineExercisesController(IRoutineExerciseService service)
	{
		_service = service;
	}

	/// <summary>
	/// Retrieves all routine–exercise links.
	/// </summary>
	[HttpGet]
	public async Task<ActionResult<IEnumerable<RoutineExerciseDto>>> GetAll()
	{
		var list = await _service.GetAllRoutineExercisesAsync();
		return Ok(list);
	}

	/// <summary>
	/// Retrieves a single routine–exercise link by its ID.
	/// </summary>
	[HttpGet("{id:int}")]
	public async Task<ActionResult<RoutineExerciseDto>> Get(int id)
	{
		var dto = await _service.GetRoutineExerciseByIdAsync(id);
		if (dto is null)
			return NotFound();
		return Ok(dto);
	}

	/// <summary>
	/// Creates a new routine–exercise link.
	/// </summary>
	[HttpPost]
	public async Task<ActionResult<RoutineExerciseDto>> Create(CreateRoutineExerciseRequest request)
	{
		var created = await _service.CreateExerciseToRoutineAsync(request);
		return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
	}

	/// <summary>
	/// Updates an existing routine-exercise link.
	/// </summary>
	[HttpPut("{id:int}")]
	public async Task<IActionResult> Update(int id, UpdateRoutineExerciseRequest request)
	{
		var updated = await _service.UpdateExerciseInRoutineAsync(id, request);
		if (!updated)
			return NotFound();
		return NoContent();
	}

	/// <summary>
	/// Deletes a routine–exercise link by its ID.
	/// </summary>
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var deleted = await _service.DeleteExerciseFromRoutineAsync(id);
		if (!deleted)
			return NotFound();
		return NoContent();
	}
}