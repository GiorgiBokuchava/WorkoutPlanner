using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner_API.Models;
using static WorkoutPlanner_API.Data.DataStore;

namespace WorkoutPlanner_API.Controllers;

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
        foreach (var exercise in Exercises)
        {
            if (exercise.Id == id)
            {
                return Ok(exercise);
            }
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
        int idx = -1;
        for (int i = 0; i < Exercises.Count; i++)
        {
            if (Exercises[i].Id == id)
            {
                idx = i;
                break;
            }
        }
        if (idx == -1)
        {
            return NotFound();
        }
        Exercises[idx] = exercise;
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        int idx = -1;
        for (int i = 0; i < Exercises.Count; i++)
        {
            if (Exercises[i].Id == id)
            {
                idx = i;
                break;
            }
        }
        if (idx == -1)
        {
            return NotFound();
        }
        Exercises.RemoveAt(idx);
        return NoContent();
    }
}
