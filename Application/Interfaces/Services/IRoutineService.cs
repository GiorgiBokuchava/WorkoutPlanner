using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Application.Interfaces.Services;

public interface IRoutineService
{
	Task<IEnumerable<RoutineDto>> GetAllRoutinesAsync();
	Task<RoutineDto?> GetRoutineByIdAsync(int id);
	Task<RoutineDto> CreateRoutineAsync(CreateRoutineRequest req);
	Task<bool> UpdateRoutineAsync(int id, UpdateRoutineRequest req);
	Task<bool> DeleteRoutineAsync(int id);
	Task<IEnumerable<RoutineDto>> GetRoutineByUserIdAsync(int userId);
}
