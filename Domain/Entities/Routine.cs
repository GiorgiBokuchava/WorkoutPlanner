namespace WorkoutPlanner.Domain.Entities;
public class Routine
{
	public int Id { get; set; }
	public int UserId { get; set; }
	public User User { get; set; } = null!;

	public string Title { get; set; } = string.Empty;
	public int FrequencyPerWeek { get; set; }
	public string Difficulty { get; set; } = string.Empty;

	public ICollection<RoutineExercise> RoutineExercises { get; set; } = new List<RoutineExercise>();
	public ICollection<WorkoutLog> WorkoutLogs { get; set; } = new List<WorkoutLog>();
}
