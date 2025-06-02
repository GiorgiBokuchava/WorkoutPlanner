namespace WorkoutPlanner.Contracts;

public record CreateWorkoutLogRequest(
	int RoutineId,
	DateTime Date,
	string? Notes
);

public record UpdateWorkoutLogRequest(
	int RoutineId,
	DateTime Date,
	string? Notes
);

public record WorkoutLogDto(
	int Id,
	int UserId,
	int RoutineId,
	DateTime Date,
	string? Notes
);