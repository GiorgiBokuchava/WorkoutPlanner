﻿namespace WorkoutPlanner.Domain.Entities;

public class Exercise
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Equipment { get; set; } = string.Empty;

	public string Target { get; set; } = string.Empty;

	public ICollection<RoutineExercise> RoutineExercises { get; set; } = new List<RoutineExercise>();
	public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
}
