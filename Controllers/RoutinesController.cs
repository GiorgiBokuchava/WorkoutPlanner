using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner_API.Data;
using WorkoutPlanner_API.Models;

namespace WorkoutPlanner_API.Controllers
{
    [ApiController]
    [Route("api/routines")]
    public class RoutinesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Routine>> GetAll()
            => Ok(DataStore.Routines);

        [HttpGet("{id:int}")]
        public ActionResult<Routine> Get(int id)
        {
            var routine = DataStore.Routines.FirstOrDefault(r => r.Id == id);
            if (routine == null) return NotFound();
            return Ok(routine);
        }

        [HttpGet("users/{userId:int}")]
        public ActionResult<IEnumerable<Routine>> GetByUser(int userId)
            => Ok(DataStore.Routines.Where(r => r.UserId == userId));

        [HttpPost]
        public ActionResult<Routine> Create(Routine routine, [FromQuery] int userId)
        {
            routine.UserId = userId;
            routine.Id = DataStore.Routines.Count + 1;
            DataStore.Routines.Add(routine);
            return CreatedAtAction(nameof(Get), new { id = routine.Id }, routine);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, Routine routine, [FromQuery] int userId)
        {
            var existing = DataStore.Routines.FirstOrDefault(r => r.Id == id);
            if (existing == null) return NotFound();

            existing.UserId = userId;
            existing.Title = routine.Title;
            existing.FrequencyPerWeek = routine.FrequencyPerWeek;
            existing.Difficulty = routine.Difficulty;
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var existing = DataStore.Routines.FirstOrDefault(r => r.Id == id);
            if (existing == null) return NotFound();

            DataStore.Routines.Remove(existing);
            return NoContent();
        }
    }
}
