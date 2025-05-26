using WorkoutPlanner_API.Models;

namespace WorkoutPlanner_API.Application.Interfaces
{
	public interface IWorkoutLogRepository
	{
		Task<WorkoutLog?> GetWorkoutLogByIdAsync(int workoutLogId);
		Task<IEnumerable<WorkoutLog>> GetWorkoutLogsByUserIdAsync(int userId);
		Task<IEnumerable<WorkoutLog>> GetAllWorkoutLogsAsync();
		Task AddWorkoutLogAsync(WorkoutLog workoutLog);
		Task UpdateWorkoutLogAsync(WorkoutLog workoutLog);
		Task DeleteWorkoutLogAsync(int workoutLogId);
		Task<IEnumerable<WorkoutExercise>> GetExercisesByWorkoutLogIdAsync(int workoutLogId);
		Task AddExerciseToWorkoutLogAsync(WorkoutExercise workoutExercise);
		Task UpdateExerciseInWorkoutLogAsync(WorkoutExercise workoutExercise);
		Task DeleteExerciseFromWorkoutLogAsync(int exerciseId, int workoutLogId);
	}
}