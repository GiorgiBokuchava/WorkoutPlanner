using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Application.Interfaces.Services;

public interface IWorkoutLogService
{
	Task<IEnumerable<WorkoutLogDto>> GetAllWorkoutLogsAsync();
	Task<WorkoutLogDto?> GetWorkoutLogByIdAsync(int id);
	Task<WorkoutLogDto> CreateWorkoutLogAsync(int userId, CreateWorkoutLogRequest request);
	Task<bool> UpdateWorkoutLogAsync(int id, UpdateWorkoutLogRequest request);
	Task<bool> DeleteWorkoutLogAsync(int id);
	Task<IEnumerable<WorkoutLogDto>> GetWorkoutLogsByUserIdAsync(int userId);
}
