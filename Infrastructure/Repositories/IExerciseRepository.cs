using WorkoutPlanner.Domain.Entities;

namespace WorkoutPlanner.Infrastructure.Repositories;
public interface IExerciseRepository
{
	Task<Exercise?> GetExerciseByIdAsync(int exerciseId);
	Task<IEnumerable<Exercise>> GetAllExercisesAsync();
	Task<int> AddExerciseAsync(Exercise exercise);
	Task UpdateExerciseAsync(Exercise exercise);
	Task DeleteExerciseAsync(int exerciseId);
	Task<IEnumerable<Exercise>> GetExercisesByTargetAsync(string target);
	Task<IEnumerable<Exercise>> GetExercisesByEquipmentAsync(string equipment);
}