using WorkoutPlanner.Models;

namespace WorkoutPlanner.Application.Interfaces;
public interface IExerciseRepository
{
	Task<Exercise?> GetExerciseByIdAsync(int exerciseId);
	Task<IEnumerable<Exercise>> GetAllExercisesAsync();
	Task AddExerciseAsync(Exercise exercise);
	Task UpdateExerciseAsync(Exercise exercise);
	Task DeleteExerciseAsync(int exerciseId);
	Task<IEnumerable<Exercise>> GetExercisesByTargetAsync(string target);
	Task<IEnumerable<Exercise>> GetExercisesByEquipmentAsync(string equipment);
}