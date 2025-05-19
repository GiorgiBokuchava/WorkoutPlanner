using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner_API.Models;
using static WorkoutPlanner_API.Data.DataStore;

namespace WorkoutPlanner_API.Controllers;

/// <summary>
/// CRUD operations for WorkoutLog resources.
/// </summary>
[ApiController]
[Route("api/workoutlogs")]
public class WorkoutLogsController : ControllerBase
{
    /// <summary>
    /// Returns all workout logs.
    /// </summary>
    [HttpGet]
    public IEnumerable<WorkoutLog> GetAll()
    {
        return WorkoutLogs;
    }

    /// <summary>
    /// Returns a workout log by ID.
    /// </summary>
    [HttpGet("{id:int}")]
    public ActionResult<WorkoutLog> Get(int id)
    {
        foreach (var log in WorkoutLogs)
        {
            if (log.Id == id)
            {
                return Ok(log);
            }
        }
        return NotFound();
    }

    /// <summary>
    /// Creates a new workout log.
    /// </summary>
    [HttpPost]
    public ActionResult<WorkoutLog> Create(WorkoutLog log)
    {
        log.Id = WorkoutLogs.Count + 1;
        WorkoutLogs.Add(log);
        return CreatedAtAction(nameof(Get), new { id = log.Id }, log);
    }

    /// <summary>
    /// Updates an existing workout log.
    /// </summary>
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, WorkoutLog log)
    {
        int idx = -1;
        for (int i = 0; i < WorkoutLogs.Count; i++)
        {
            if (WorkoutLogs[i].Id == id)
            {
                idx = i;
                break;
            }
        }

        if (idx == -1)
        {
            return NotFound();
        }

        log.Id = id;
        WorkoutLogs[idx] = log;
        return NoContent();
    }

    /// <summary>
    /// Deletes a workout log by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        for (int i = 0; i < WorkoutLogs.Count; i++)
        {
            if (WorkoutLogs[i].Id == id)
            {
                WorkoutLogs.RemoveAt(i);
                return NoContent();
            }
        }
        return NotFound();
    }
}
