using WorkoutPlanner.Domain.Entities;
using WorkoutPlanner.Contracts;
using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Infrastructure.Repositories;

namespace WorkoutPlanner.Application.Services;

public class ExerciseService : IExerciseService
{
	public readonly IExerciseRepository _exerciseRepository;

	public ExerciseService(IExerciseRepository exerciseRepository)
	{
		_exerciseRepository = exerciseRepository;
	}

	public async Task<IEnumerable<ExerciseDto>> GetAllExercisesAsync()
	{
		var exercises = await _exerciseRepository.GetAllExercisesAsync();
		return exercises.Select(e => new ExerciseDto(e.Id, e.Name, e.Equipment, e.Target));
	}

	public async Task<ExerciseDto?> GetExerciseByIdAsync(int id)
	{
		var exercise = await _exerciseRepository.GetExerciseByIdAsync(id);
		if (exercise is null) return null;

		return new ExerciseDto(exercise.Id, exercise.Name, exercise.Equipment, exercise.Target);
	}

	public async Task<ExerciseDto> CreateExerciseAsync(CreateExerciseRequest request)
	{
		var exercise = new Exercise
		{
			Name = request.Name,
			Equipment = request.Equipment,
			Target = request.Target
		};
		var id = await _exerciseRepository.AddExerciseAsync(exercise);
		exercise.Id = id;

		return new ExerciseDto(exercise.Id, exercise.Name, exercise.Equipment, exercise.Target);
	}

	public async Task<bool> UpdateExerciseAsync(int id, UpdateExerciseRequest request)
	{
		var existing = await _exerciseRepository.GetExerciseByIdAsync(id);
		if (existing is null) return false;

		existing.Name = request.Name;
		existing.Equipment = request.Equipment;
		existing.Target = request.Target;

		await _exerciseRepository.UpdateExerciseAsync(existing);
		return true;
	}

	public async Task<bool> DeleteExerciseAsync(int id)
	{
		var existing = await _exerciseRepository.GetExerciseByIdAsync(id);

		if (existing is null) return false;

		await _exerciseRepository.DeleteExerciseAsync(id);
		return true;
	}
}