using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WorkoutPlanner_API.Models
{
    /// <summary>
    /// Represents a logged workout session with its associated routine and exercises
    /// </summary>
    [Table("WorkoutLogs", Schema = "Workout")]
    [Index(nameof(UserId))]
    [Index(nameof(RoutineId))]
    public class WorkoutLog
    {
        /// <summary>
        /// Unique identifier for the workout log
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// ID of the user who performed the workout
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Navigation property to the associated user
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        /// <summary>
        /// ID of the routine that was performed
        /// </summary>
        [Required]
        public int RoutineId { get; set; }

        /// <summary>
        /// Navigation property to the associated routine
        /// </summary>
        [ForeignKey(nameof(RoutineId))]
        public Routine Routine { get; set; } = null!;

        /// <summary>
        /// Date and time when the workout was performed
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Optional notes about the workout session
        /// </summary>
        [MaxLength(500)]
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        /// Collection of exercises performed during this workout
        /// </summary>
        public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
    }
}
