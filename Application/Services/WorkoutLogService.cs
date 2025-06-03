using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Contracts;
using WorkoutPlanner.Domain.Entities;
using WorkoutPlanner.Infrastructure.Repositories;

namespace WorkoutPlanner.Application.Services;

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

	public async Task<WorkoutLogDto?> GetWorkoutLogByIdAsync(int id)
	{
		var log = await _workoutLogRepository.GetWorkoutLogByIdAsync(id);
		if (log is null) return null;

		return new WorkoutLogDto(log.Id, log.UserId, log.RoutineId, log.Date, log.Notes);
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
	public async Task<bool> UpdateWorkoutLogAsync(int id, UpdateWorkoutLogRequest request)
	{
		var existing = await _workoutLogRepository.GetWorkoutLogByIdAsync(id);
		if (existing is null) return false;

		existing.RoutineId = request.RoutineId;
		existing.Date = request.Date;
		existing.Notes = request.Notes ?? string.Empty;

		await _workoutLogRepository.UpdateWorkoutLogAsync(existing);
		return true;
	}

	public async Task<bool> DeleteWorkoutLogAsync(int id)
	{
		var existing = await _workoutLogRepository.GetWorkoutLogByIdAsync(id);
		if (existing is null) return false;

		await _workoutLogRepository.DeleteWorkoutLogAsync(id);
		return true;
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