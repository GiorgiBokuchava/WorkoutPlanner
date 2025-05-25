using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner_API.Domain.Entities;
using WorkoutPlanner_API.Application.Interfaces;

namespace WorkoutPlanner_API.Controllers
{
	/// <summary>
	/// Manages workout log operations
	/// </summary>
	[ApiController]
	[Route("api/workoutLogs")]
	[Produces("application/json")]
	public class WorkoutLogsController : ControllerBase
	{
		private readonly IWorkoutLogRepository _logs;

		public WorkoutLogsController(IWorkoutLogRepository logs)
		{
			_logs = logs;
		}

		/// <summary>
		/// Gets all workout logs
		/// </summary>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<WorkoutLog>>> GetAll()
		{
			var logs = await _logs.GetAllWorkoutLogsAsync();
			return Ok(logs);
		}

		/// <summary>
		/// Gets workout log by ID
		/// </summary>
		/// <param name="id">Workout log ID</param>
		/// <response code="404">Log not found</response>
		[HttpGet("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<WorkoutLog>> Get(int id)
		{
			var log = await _logs.GetWorkoutLogByIdAsync(id);
			if (log == null) return NotFound();
			return Ok(log);
		}

		/// <summary>
		/// Gets all logs for a user
		/// </summary>
		/// <param name="userId">User ID</param>
		[HttpGet("users/{userId:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<WorkoutLog>>> GetByUser(int userId)
		{
			var logs = await _logs.GetWorkoutLogsByUserIdAsync(userId);
			return Ok(logs);
		}

		/// <summary>
		/// Creates a workout log
		/// </summary>
		/// <param name="log">Workout log data</param>
		/// <param name="userId">User ID</param>
		/// <response code="400">Invalid request</response>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<WorkoutLog>> Create(WorkoutLog log, [FromQuery] int userId)
		{
			log.UserId = userId;
			await _logs.AddWorkoutLogAsync(log);
			return CreatedAtAction(nameof(Get), new { id = log.Id }, log);
		}

		/// <summary>
		/// Updates a workout log
		/// </summary>
		/// <param name="id">Log ID</param>
		/// <param name="log">Updated data</param>
		/// <param name="userId">User ID</param>
		/// <response code="404">Log not found</response>
		[HttpPut("{id:int}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Update(int id, WorkoutLog log, [FromQuery] int userId)
		{
			log.Id = id;
			log.UserId = userId;
			await _logs.UpdateWorkoutLogAsync(log);
			return NoContent();
		}

		/// <summary>
		/// Deletes a workout log
		/// </summary>
		/// <param name="id">Log ID</param>
		/// <response code="404">Log not found</response>
		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete(int id)
		{
			await _logs.DeleteWorkoutLogAsync(id);
			return NoContent();
		}
	}
}
