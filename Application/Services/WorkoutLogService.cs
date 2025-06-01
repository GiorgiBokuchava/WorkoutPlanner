using WorkoutPlanner.Application.Interfaces;
using WorkoutPlanner.Contracts;
using WorkoutPlanner.Domain.Entities;

namespace WorkoutPlanner.Application.Services;

public interface IWorkoutLogService
{
	Task<IEnumerable<WorkoutLogDto>> GetAllWorkoutLogsAsync();
	Task<WorkoutLogDto> GetWorkoutLogByIdAsync(int id);
	Task<WorkoutLogDto> CreateWorkoutLogAsync(int userId, CreateWorkoutLogRequest request);
	Task UpdateWorkoutLogAsync(int id, CreateWorkoutLogRequest request);
	Task DeleteWorkoutLogAsync(int id);
	Task<IEnumerable<WorkoutLogDto>> GetWorkoutLogsByUserIdAsync(int userId);
}

public class WorkoutLogService : IWorkoutLogService
{
	private readonly IWorkoutLogRepository _workoutLogRepository;
	public WorkoutLogService(IWorkoutLogRepository workoutLogRepository)
	{
		_workoutLogRepository = workoutLogRepository;
	}

	public async Task<IEnumerable<WorkoutLogDto>> GetAllWorkoutLogsAsync()
	{
		var workoutLogs = await _workoutLogRepository.GetAllWorkoutLogsAsync();
		return workoutLogs.Select(log => new WorkoutLogDto(
			log.Id,
			log.UserId,
			log.RoutineId,
			log.Date,
			log.Notes
		));
	}

	public async Task<WorkoutLogDto> GetWorkoutLogByIdAsync(int id)
	{
		var entity = await _workoutLogRepository.GetWorkoutLogByIdAsync(id);
		if (entity == null) throw new KeyNotFoundException($"Workout log with ID {id} not found.");
		return new WorkoutLogDto(
			entity.Id,
			entity.UserId,
			entity.RoutineId,
			entity.Date,
			entity.Notes
		);
	}
	public async Task<WorkoutLogDto> CreateWorkoutLogAsync(int userId, CreateWorkoutLogRequest request)
	{
		var workoutLog = new WorkoutLog
		{
			UserId = userId,
			RoutineId = request.RoutineId,
			Date = request.Date,
			Notes = request.Notes ?? string.Empty
		};
		workoutLog.Id = await _workoutLogRepository.AddWorkoutLogAsync(workoutLog);

		return new WorkoutLogDto(
			workoutLog.Id,
			workoutLog.UserId,
			workoutLog.RoutineId,
			workoutLog.Date,
			workoutLog.Notes
		);
	}
	public async Task UpdateWorkoutLogAsync(int id, CreateWorkoutLogRequest request)
	{
		var existing = await _workoutLogRepository.GetWorkoutLogByIdAsync(id) ?? throw new KeyNotFoundException($"Workout log with ID {id} not found.");

		var updated = new WorkoutLog
		{
			Id = id,
			RoutineId = request.RoutineId,
			Date = request.Date,
			Notes = request.Notes ?? string.Empty
		};

		await _workoutLogRepository.UpdateWorkoutLogAsync(updated);
	}

	public async Task DeleteWorkoutLogAsync(int id)
	{
		var existingLog = await _workoutLogRepository.GetWorkoutLogByIdAsync(id)
			?? throw new KeyNotFoundException($"Workout log with ID {id} not found.");

		await _workoutLogRepository.DeleteWorkoutLogAsync(id);
	}

	public async Task<IEnumerable<WorkoutLogDto>> GetWorkoutLogsByUserIdAsync(int userId)
	{
		var workoutLogs = await _workoutLogRepository.GetWorkoutLogsByUserIdAsync(userId);

		return workoutLogs.Select(log => new WorkoutLogDto(
			log.Id,
			log.UserId,
			log.RoutineId,
			log.Date,
			log.Notes
		));
	}
}