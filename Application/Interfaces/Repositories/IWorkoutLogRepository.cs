using WorkoutPlanner.Domain.Entities;

namespace WorkoutPlanner.Application.Interfaces.Repositories;
public interface IWorkoutLogRepository
{
	// WorkoutLog methods
	Task<IEnumerable<WorkoutLog>> GetAllWorkoutLogsAsync();
	Task<WorkoutLog?> GetWorkoutLogByIdAsync(int workoutLogId);
	Task<IEnumerable<WorkoutLog>> GetWorkoutLogsByUserIdAsync(int userId);
	Task<int> AddWorkoutLogAsync(WorkoutLog workoutLog);
	Task UpdateWorkoutLogAsync(WorkoutLog workoutLog);
	Task DeleteWorkoutLogAsync(int workoutLogId);

	// WorkoutExercise methods
	Task<IEnumerable<WorkoutExercise>> GetAllWorkoutExercisesAsync();
	Task<WorkoutExercise?> GetWorkoutExerciseByIdAsync(int workoutExerciseId);
	Task<IEnumerable<WorkoutExercise>> GetExercisesByWorkoutLogIdAsync(int workoutLogId);
	Task<int> AddExerciseToWorkoutLogAsync(WorkoutExercise workoutExercise);
	Task UpdateExerciseInWorkoutLogAsync(WorkoutExercise workoutExercise);
	Task DeleteExerciseFromWorkoutLogAsync(int exerciseId, int workoutLogId);
}