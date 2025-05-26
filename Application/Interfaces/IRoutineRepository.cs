using WorkoutPlanner_API.Models;

namespace WorkoutPlanner_API.Application.Interfaces
{
	public interface IRoutineRepository
	{
		Task<Routine?> GetRoutineByIdAsync(int routineId);
		Task<IEnumerable<Routine>> GetRoutinesByUserIdAsync(int userId);
		Task AddRoutineAsync(Routine routine);
		Task UpdateRoutineAsync(Routine routine);
		Task DeleteRoutineAsync(int routineId);
		Task<IEnumerable<RoutineExercise>> GetExercisesByRoutineIdAsync(int routineId);
		Task AddExerciseToRoutineAsync(RoutineExercise routineExercise);
		Task UpdateExerciseInRoutineAsync(RoutineExercise routineExercise);
		Task DeleteExerciseFromRoutineAsync(int exerciseId, int routineId);
	}
}