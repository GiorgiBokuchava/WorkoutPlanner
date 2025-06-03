using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Application.Interfaces.Services;

public interface IWorkoutExerciseService
{
	Task<IEnumerable<WorkoutExerciseDto>> GetAllWorkoutExercisesAsync();
	Task<WorkoutExerciseDto?> GetWorkoutExerciseByIdAsync(int id);
	Task<WorkoutExerciseDto> CreateWorkoutExerciseAsync(CreateWorkoutExerciseRequest request);
	Task<bool> UpdateWorkoutExerciseAsync(int id, UpdateWorkoutExerciseRequest request);
	Task<bool> DeleteWorkoutExerciseAsync(int id);
}
