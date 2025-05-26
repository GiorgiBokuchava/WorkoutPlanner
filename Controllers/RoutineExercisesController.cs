using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner_API.Data;
using WorkoutPlanner_API.Models;

namespace WorkoutPlanner_API.Controllers
{
    [ApiController]
    [Route("api/routineExercises")]
    public class RoutineExercisesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<RoutineExercise>> GetAll()
            => Ok(DataStore.RoutineExercises);

        [HttpGet("{id:int}")]
        public ActionResult<RoutineExercise> Get(int id)
        {
            var re = DataStore.RoutineExercises.FirstOrDefault(x => x.Id == id);
            if (re == null) return NotFound();
            return Ok(re);
        }

        [HttpPost]
        public ActionResult<RoutineExercise> Create(RoutineExercise re)
        {
            re.Id = DataStore.RoutineExercises.Count + 1;
            DataStore.RoutineExercises.Add(re);
            return CreatedAtAction(nameof(Get), new { id = re.Id }, re);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, RoutineExercise re)
        {
            var existing = DataStore.RoutineExercises.FirstOrDefault(x => x.Id == id);
            if (existing == null) return NotFound();

            existing.RoutineId = re.RoutineId;
            existing.ExerciseId = re.ExerciseId;
            existing.Sets = re.Sets;
            existing.RepsPerSet = re.RepsPerSet;
            existing.Weight = re.Weight;
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var existing = DataStore.RoutineExercises.FirstOrDefault(x => x.Id == id);
            if (existing == null) return NotFound();

            DataStore.RoutineExercises.Remove(existing);
            return NoContent();
        }
    }
}
