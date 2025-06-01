namespace WorkoutPlanner.Contracts;

public record CreateRoutineExerciseRequest(
	int RoutineId,
	int ExerciseId,
	int Sets = 0,
	int RepsPerSet = 0,
	decimal? Weight = null
);

public record RoutineExerciseDto(
	int Id,
	int RoutineId,
	int ExerciseId,
	int Sets,
	int RepsPerSet,
	decimal? Weight
);