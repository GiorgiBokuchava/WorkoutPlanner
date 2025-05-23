using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WorkoutPlanner_API.Models
{
    [Table("WorkoutExercises", Schema = "Workout")]
    [Index(nameof(WorkoutLogId))]
    [Index(nameof(ExerciseId))]
    public class WorkoutExercise
    {
        [Key] public int Id { get; set; }

        public int WorkoutLogId { get; set; }
        [ForeignKey(nameof(WorkoutLogId))]
        public WorkoutLog WorkoutLog { get; set; } = null!;

        public int ExerciseId { get; set; }
        [ForeignKey(nameof(ExerciseId))]
        public Exercise Exercise { get; set; } = null!;

        public int SetsCompleted { get; set; }
        public int RepsCompleted { get; set; }
        public decimal? WeightUsed { get; set; }
    }
}