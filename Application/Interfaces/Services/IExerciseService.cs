using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Application.Interfaces.Services;

public interface IExerciseService
{
	Task<IEnumerable<ExerciseDto>> GetAllExercisesAsync();
	Task<ExerciseDto?> GetExerciseByIdAsync(int id);
	Task<ExerciseDto> CreateExerciseAsync(CreateExerciseRequest request);
	Task<bool> UpdateExerciseAsync(int id, UpdateExerciseRequest request);
	Task<bool> DeleteExerciseAsync(int id);
}
