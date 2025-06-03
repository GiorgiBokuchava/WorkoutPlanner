using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Application.Interfaces.Services;

public interface IRoutineExerciseService
{
	Task<IEnumerable<RoutineExerciseDto>> GetAllRoutineExercisesAsync();
	Task<RoutineExerciseDto?> GetRoutineExerciseByIdAsync(int id);
	Task<IEnumerable<RoutineExerciseDto>> GetExercisesByRoutineIdAsync(int id);
	Task<RoutineExerciseDto> CreateExerciseToRoutineAsync(CreateRoutineExerciseRequest request);
	Task<bool> UpdateExerciseInRoutineAsync(int id, UpdateRoutineExerciseRequest request);
	Task<bool> DeleteExerciseFromRoutineAsync(int id);
}
