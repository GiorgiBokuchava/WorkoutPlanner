using WorkoutPlanner.Domain.Entities;

namespace WorkoutPlanner.Application.Interfaces;
public interface IRoutineRepository
{
	// Routine only methods
	Task<IEnumerable<Routine>> GetAllRoutinesAsync();
	Task<Routine?> GetRoutineByIdAsync(int routineId);
	Task<IEnumerable<Routine>> GetRoutinesByUserIdAsync(int userId);
	Task<int> AddRoutineAsync(Routine routine);
	Task UpdateRoutineAsync(Routine routine);
	Task DeleteRoutineAsync(int routineId);

	// Routine <-> Exercise methods
	Task<IEnumerable<RoutineExercise>> GetAllRoutineExercisesAsync();
	Task<RoutineExercise?> GetRoutineExerciseByIdAsync(int routineExerciseId);
	Task<IEnumerable<RoutineExercise>> GetExercisesByRoutineIdAsync(int routineId);
	Task<int> AddExerciseToRoutineAsync(RoutineExercise routineExercise);
	Task UpdateExerciseInRoutineAsync(RoutineExercise routineExercise);
	Task DeleteExerciseFromRoutineAsync(int exerciseId, int routineId);
}