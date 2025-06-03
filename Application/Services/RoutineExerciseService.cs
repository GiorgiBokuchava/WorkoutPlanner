using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Contracts;
using WorkoutPlanner.Domain.Entities;
using WorkoutPlanner.Infrastructure.Repositories;

namespace WorkoutPlanner.Application.Services;

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

	public async Task<RoutineExerciseDto?> GetRoutineExerciseByIdAsync(int id)
	{
		var e = await _routineExerciseRepository.GetRoutineExerciseByIdAsync(id);
		if (e is null) return null;

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

	public async Task<bool> UpdateExerciseInRoutineAsync(int id, UpdateRoutineExerciseRequest request)
	{
		var existing = await _routineExerciseRepository.GetRoutineExerciseByIdAsync(id);
		if (existing is null) return false;

		existing.Sets = request.Sets;
		existing.RepsPerSet = request.RepsPerSet;
		existing.Weight = request.Weight;

		await _routineExerciseRepository.UpdateExerciseInRoutineAsync(existing);
		return true;
	}

	public async Task<bool> DeleteExerciseFromRoutineAsync(int id)
	{
		var existing = await _routineExerciseRepository.GetRoutineExerciseByIdAsync(id);
		if (existing is null) return false;

		await _routineExerciseRepository.DeleteExerciseFromRoutineAsync(existing.ExerciseId, existing.RoutineId);
		return true;
	}
}