using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner_API.Models;
using static WorkoutPlanner_API.Data.DataStore;

namespace WorkoutPlanner_API.Controllers;

/// <summary>
/// Basic CRUD operations for User resources
/// </summary>
[ApiController]
[Route("api/routines")]
public class RoutinesController : ControllerBase
{
    /// <summary>
    /// Returns all routines
    /// </summary>
    [HttpGet]
    public IEnumerable<Routine> GetAll()
    {
        return Routines;
    }

    /// <summary>
    /// Returns a routine by ID
    /// </summary>
    [HttpGet("{id:int}")]
    public ActionResult<Routine> Get(int id)
    {
        var routine = Routines.FirstOrDefault(r => r.Id == id);

        if (routine != null)
        {
            return Ok(routine);
        }

        return NotFound();
    }

    /// <summary>
    /// Creates a new routine
    /// </summary>
    [HttpPost]
    public ActionResult<Routine> Create(Routine routine)
    {
        routine.Id = Routines.Count + 1;
        Routines.Add(routine);
        return CreatedAtAction(nameof(Get), new { id = routine.Id }, routine);
    }

    /// <summary>
    /// Updates an existing routine
    /// </summary>
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, Routine routine)
    {
        var initial = Routines.FirstOrDefault(r => r.Id == id);

        if (initial == null)
        {
            return NotFound();
        }

        initial.Title = routine.Title;
        initial.FrequencyPerWeek = routine.FrequencyPerWeek;
        initial.Difficulty = routine.Difficulty;

        return NoContent();
    }

    /// <summary>
    /// Deletes a routine by ID
    /// </summary>
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var routine = Routines.FirstOrDefault(r => r.Id == id);

        if (routine == null)
        {
            return NotFound();
        }

        Routines.Remove(routine);

        return NoContent();
    }
}
