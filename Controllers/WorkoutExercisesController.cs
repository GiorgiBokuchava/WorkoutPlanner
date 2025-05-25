using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner_API.Domain.Entities;

namespace WorkoutPlanner_API.Controllers
{
    [ApiController]
    [Route("api/workoutExercises")]
    public class WorkoutExercisesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<WorkoutExercise>> GetAll()
            => Ok(DataStore.WorkoutExercises);

        [HttpGet("{id:int}")]
        public ActionResult<WorkoutExercise> Get(int id)
        {
            var we = DataStore.WorkoutExercises.FirstOrDefault(x => x.Id == id);
            if (we == null) return NotFound();
            return Ok(we);
        }

        [HttpPost]
        public ActionResult<WorkoutExercise> Create(WorkoutExercise we)
        {
            we.Id = DataStore.WorkoutExercises.Count + 1;
            DataStore.WorkoutExercises.Add(we);
            return CreatedAtAction(nameof(Get), new { id = we.Id }, we);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, WorkoutExercise we)
        {
            var existing = DataStore.WorkoutExercises.FirstOrDefault(x => x.Id == id);
            if (existing == null) return NotFound();

            existing.WorkoutLogId = we.WorkoutLogId;
            existing.ExerciseId = we.ExerciseId;
            existing.SetsCompleted = we.SetsCompleted;
            existing.RepsCompleted = we.RepsCompleted;
            existing.WeightUsed = we.WeightUsed;
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var existing = DataStore.WorkoutExercises.FirstOrDefault(x => x.Id == id);
            if (existing == null) return NotFound();

            DataStore.WorkoutExercises.Remove(existing);
            return NoContent();
        }
    }
}
