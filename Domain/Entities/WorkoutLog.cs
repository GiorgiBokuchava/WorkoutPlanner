namespace WorkoutPlanner.Models;

/// <summary>
/// Represents a logged workout session with its associated routine and exercises
/// </summary>
public class WorkoutLog
{
	/// <summary>
	/// Unique identifier for the workout log
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// ID of the user who performed the workout
	/// </summary>
	public int UserId { get; set; }

	/// <summary>
	/// Navigation property to the associated user
	/// </summary>
	public User User { get; set; } = null!;

	/// <summary>
	/// ID of the routine that was performed
	/// </summary>
	public int RoutineId { get; set; }

	/// <summary>
	/// Navigation property to the associated routine
	/// </summary>
	public Routine Routine { get; set; } = null!;

	/// <summary>
	/// Date and time when the workout was performed
	/// </summary>
	public DateTime Date { get; set; }

	/// <summary>
	/// Optional notes about the workout session
	/// </summary>
	public string Notes { get; set; } = string.Empty;

	/// <summary>
	/// Collection of exercises performed during this workout
	/// </summary>
	public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
}
