using WorkoutPlanner.Application.Interfaces;
using WorkoutPlanner.Contracts;
using WorkoutPlanner.Domain.Entities;

namespace WorkoutPlanner.Application.Services;

public interface IRoutineExerciseService
{
	Task<IEnumerable<RoutineExerciseDto>> GetAllRoutineExercisesAsync();
	Task<RoutineExerciseDto> GetRoutineExerciseByIdAsync(int id);
	Task<IEnumerable<RoutineExerciseDto>> GetExercisesByRoutineIdAsync(int id);
	Task<RoutineExerciseDto> CreateExerciseToRoutineAsync(CreateRoutineExerciseRequest request);
	Task UpdateExerciseInRoutineAsync(int id, CreateRoutineExerciseRequest request);
	Task DeleteExerciseFromRoutineAsync(int id);
}

public class RoutineExerciseService : IRoutineExerciseService
{
	private readonly IRoutineRepository _routineExerciseRepository;

	public RoutineExerciseService(IRoutineRepository routineExerciseRepository)
	{
		_routineExerciseRepository = routineExerciseRepository;
	}

	public async Task<IEnumerable<RoutineExerciseDto>> GetAllRoutineExercisesAsync()
	{
		var entities = await _routineExerciseRepository.GetAllRoutineExercisesAsync();

		return entities
			.Select(e =>
				new RoutineExerciseDto(
					e.Id,
					e.RoutineId,
					e.ExerciseId,
					e.Sets,
					e.RepsPerSet,
					e.Weight
				)
			);
	}

	public async Task<RoutineExerciseDto> GetRoutineExerciseByIdAsync(int id)
	{
		var e = await _routineExerciseRepository.GetRoutineExerciseByIdAsync(id);
		if (e is null)
			throw new KeyNotFoundException($"RoutineExercise with ID {id} not found.");

		return new RoutineExerciseDto(
			e.Id,
			e.RoutineId,
			e.ExerciseId,
			e.Sets,
			e.RepsPerSet,
			e.Weight
		);
	}

	public async Task<IEnumerable<RoutineExerciseDto>> GetExercisesByRoutineIdAsync(int id)
	{
		var entities = await _routineExerciseRepository.GetExercisesByRoutineIdAsync(id);
		return entities.Select(e =>
				new RoutineExerciseDto(
					e.Id,
					e.RoutineId,
					e.ExerciseId,
					e.Sets,
					e.RepsPerSet,
					e.Weight
				)
			);
	}

	public async Task<RoutineExerciseDto> CreateExerciseToRoutineAsync(CreateRoutineExerciseRequest request)
	{
		var routineExercise = new RoutineExercise
		{
			RoutineId = request.RoutineId,
			ExerciseId = request.ExerciseId,
			Sets = request.Sets,
			RepsPerSet = request.RepsPerSet,
			Weight = request.Weight
		};

		routineExercise.Id = await _routineExerciseRepository.AddExerciseToRoutineAsync(routineExercise);

		return new RoutineExerciseDto(
			routineExercise.Id,
			routineExercise.RoutineId,
			routineExercise.ExerciseId,
			routineExercise.Sets,
			routineExercise.RepsPerSet,
			routineExercise.Weight
		);
	}

	public async Task UpdateExerciseInRoutineAsync(int id, CreateRoutineExerciseRequest request)
	{
		var existing = await _routineExerciseRepository.GetRoutineExerciseByIdAsync(id)
			?? throw new KeyNotFoundException($"RoutineExercise with ID {id} not found.");

		existing.Sets = request.Sets;
		existing.RepsPerSet = request.RepsPerSet;
		existing.Weight = request.Weight;

		await _routineExerciseRepository.UpdateExerciseInRoutineAsync(existing);
	}

	public async Task DeleteExerciseFromRoutineAsync(int id)
	{
		var existing = await _routineExerciseRepository.GetRoutineExerciseByIdAsync(id)
			?? throw new KeyNotFoundException($"RoutineExercise with ID {id} not found.");

		await _routineExerciseRepository.DeleteExerciseFromRoutineAsync(existing.ExerciseId, existing.RoutineId);
	}
}