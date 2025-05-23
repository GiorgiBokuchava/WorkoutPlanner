using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutPlanner_API.Models
{
    [Table("RoutineExercises", Schema = "Workout")]
    [Index(nameof(RoutineId))]
    [Index(nameof(ExerciseId))]
    public class RoutineExercise
    {
        [Key]
        public int Id { get; set; }

        public int RoutineId { get; set; }
        [ForeignKey(nameof(RoutineId))]
        public Routine Routine { get; set; } = null!;

        public int ExerciseId { get; set; }
        [ForeignKey(nameof(ExerciseId))]
        public Exercise Exercise { get; set; } = null!;

        public int Sets { get; set; }
        public int RepsPerSet { get; set; }
        public decimal? Weight { get; set; }
    }
}