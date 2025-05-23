using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WorkoutPlanner_API.Models
{
    [Table("Routines", Schema = "Workout")]
    [Index(nameof(UserId))]
    public class Routine
    {
        [Key] public int Id { get; set; }

        // FK -> Users
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        public string Title { get; set; } = string.Empty;
        public int FrequencyPerWeek { get; set; }
        public string Difficulty { get; set; } = string.Empty;

        // navigation
        public ICollection<RoutineExercise> RoutineExercises { get; set; } = new List<RoutineExercise>();
        public ICollection<WorkoutLog> WorkoutLogs { get; set; } = new List<WorkoutLog>();
    }
}
