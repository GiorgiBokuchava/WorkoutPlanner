using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Contracts;
using WorkoutPlanner.Domain.Entities;
using WorkoutPlanner.Infrastructure.Repositories;

namespace WorkoutPlanner.Application.Services;

public class RoutineService : IRoutineService
{
	private readonly IRoutineRepository _routineRepository;
	public RoutineService(IRoutineRepository routineRepository)
	{
		_routineRepository = routineRepository;
	}

	public async Task<IEnumerable<RoutineDto>> GetAllRoutinesAsync()
	{
		var routines = await _routineRepository.GetAllRoutinesAsync();
		return routines.Select(r => new RoutineDto(r.Id, r.UserId, r.Title, r.FrequencyPerWeek, r.Difficulty));
	}

	public async Task<RoutineDto?> GetRoutineByIdAsync(int id)
	{
		var routine = await _routineRepository.GetRoutineByIdAsync(id);
		if (routine is null) return null;

		return new RoutineDto(routine.Id, routine.UserId, routine.Title, routine.FrequencyPerWeek, routine.Difficulty);
	}

	public async Task<RoutineDto> CreateRoutineAsync(CreateRoutineRequest req)
	{
		var routine = new Routine
		{
			UserId = req.UserId,
			Title = req.Title,
			FrequencyPerWeek = req.FrequencyPerWeek,
			Difficulty = req.Difficulty
		};
		var id = await _routineRepository.AddRoutineAsync(routine);
		routine.Id = id;

		return new RoutineDto(routine.Id, routine.UserId, routine.Title, routine.FrequencyPerWeek, routine.Difficulty);
	}

	public async Task<bool> UpdateRoutineAsync(int id, UpdateRoutineRequest req)
	{
		var existing = await _routineRepository.GetRoutineByIdAsync(id);
		if (existing is null) return false;

		existing.Title = req.Title;
		existing.FrequencyPerWeek = req.FrequencyPerWeek;
		existing.Difficulty = req.Difficulty;

		await _routineRepository.UpdateRoutineAsync(existing);
		return true;
	}

	public async Task<bool> DeleteRoutineAsync(int id)
	{
		var existing = await _routineRepository.GetRoutineByIdAsync(id);
		if (existing is null) return false;

		await _routineRepository.DeleteRoutineAsync(id);
		return true;
	}

	public async Task<IEnumerable<RoutineDto>> GetRoutineByUserIdAsync(int userId)
	{
		var routines = await _routineRepository.GetRoutinesByUserIdAsync(userId);

		return routines.Select(r => new RoutineDto(r.Id, r.UserId, r.Title, r.FrequencyPerWeek, r.Difficulty));
	}
}