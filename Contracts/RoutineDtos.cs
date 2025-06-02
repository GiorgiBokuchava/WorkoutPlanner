namespace WorkoutPlanner.Contracts;

public record CreateRoutineRequest(
	int UserId,
	string Title,
	int FrequencyPerWeek,
	string Difficulty
);

public record UpdateRoutineRequest(
	int UserId,
	string Title,
	int FrequencyPerWeek,
	string Difficulty
);

public record RoutineDto(
	int Id,
	int UserId,
	string Title,
	int FrequencyPerWeek,
	string Difficulty
);