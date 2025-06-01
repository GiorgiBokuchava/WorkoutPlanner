namespace WorkoutPlanner.Contracts;

public record CreateWorkoutExerciseRequest(
	int WorkoutLogId,
	int ExerciseId,
	int SetsCompleted,
	int RepsCompleted,
	decimal? WeightUsed
);

public record WorkoutExerciseDto(
	int Id,
	int WorkoutLogId,
	int ExerciseId,
	int SetsCompleted,
	int RepsCompleted,
	decimal? WeightUsed
);