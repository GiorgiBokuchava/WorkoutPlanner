namespace WorkoutPlanner.Contracts;

public record CreateExerciseRequest(
	string Name,
	string Equipment,
	string Target
);

public record ExerciseDto
(
	int Id,
	string Name,
	string Equipment,
	string Target
);