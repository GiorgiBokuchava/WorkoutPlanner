using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner_API.Domain.Entities;
using WorkoutPlanner_API.Application.Interfaces;

namespace WorkoutPlanner_API.Controllers
{
	[ApiController]
	[Route("api/routines")]
	public class RoutinesController : ControllerBase
	{
		private readonly IRoutineRepository _routines;

		public RoutinesController(IRoutineRepository routines)
		{
			_routines = routines;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Routine>>> GetAll()
		{
			var routines = await _routines.GetRoutinesByUserIdAsync(0);
			return Ok(routines);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<Routine>> Get(int id)
		{
			var routine = await _routines.GetRoutineByIdAsync(id);
			if (routine == null) return NotFound();
			return Ok(routine);
		}

		[HttpGet("users/{userId:int}")]
		public async Task<ActionResult<IEnumerable<Routine>>> GetByUser(int userId)
		{
			var routines = await _routines.GetRoutinesByUserIdAsync(userId);
			return Ok(routines);
		}

		[HttpPost]
		public async Task<ActionResult<Routine>> Create(Routine routine, [FromQuery] int userId)
		{
			routine.UserId = userId;
			await _routines.AddRoutineAsync(routine);
			return CreatedAtAction(nameof(Get), new { id = routine.Id }, routine);
		}

		[HttpPut("{id:int}")]
		public async Task<IActionResult> Update(int id, Routine routine, [FromQuery] int userId)
		{
			routine.Id = id;
			routine.UserId = userId;
			await _routines.UpdateRoutineAsync(routine);
			return NoContent();
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _routines.DeleteRoutineAsync(id);
			return NoContent();
		}
	}
}
