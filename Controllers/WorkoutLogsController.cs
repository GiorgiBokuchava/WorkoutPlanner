using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner_API.Data;
using WorkoutPlanner_API.Models;

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
        /// <summary>
        /// Gets all workout logs
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<WorkoutLog>> GetAll()
            => Ok(DataStore.WorkoutLogs);

        /// <summary>
        /// Gets workout log by ID
        /// </summary>
        /// <param name="id">Workout log ID</param>
        /// <response code="404">Log not found</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<WorkoutLog> Get(int id)
        {
            var log = DataStore.WorkoutLogs.FirstOrDefault(w => w.Id == id);
            if (log == null) return NotFound();
            return Ok(log);
        }

        /// <summary>
        /// Gets all logs for a user
        /// </summary>
        /// <param name="userId">User ID</param>
        [HttpGet("users/{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<WorkoutLog>> GetByUser(int userId)
            => Ok(DataStore.WorkoutLogs.Where(w => w.UserId == userId));

        /// <summary>
        /// Creates a workout log
        /// </summary>
        /// <param name="log">Workout log data</param>
        /// <param name="userId">User ID</param>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<WorkoutLog> Create(WorkoutLog log, [FromQuery] int userId)
        {
            log.UserId = userId;
            log.Id = DataStore.WorkoutLogs.Count + 1;
            DataStore.WorkoutLogs.Add(log);
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
        public IActionResult Update(int id, WorkoutLog log, [FromQuery] int userId)
        {
            var existing = DataStore.WorkoutLogs.FirstOrDefault(w => w.Id == id);
            if (existing == null) return NotFound();

            existing.UserId = userId;
            existing.RoutineId = log.RoutineId;
            existing.Date = log.Date;
            existing.Notes = log.Notes;
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
        public IActionResult Delete(int id)
        {
            var existing = DataStore.WorkoutLogs.FirstOrDefault(w => w.Id == id);
            if (existing == null) return NotFound();

            DataStore.WorkoutLogs.Remove(existing);
            return NoContent();
        }
    }
}
