using WorkoutPlanner.Domain.Entities;
using WorkoutPlanner.Application.Interfaces;
using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Application.Services;

public interface IExerciseService
{
	Task<IEnumerable<ExerciseDto>> GetAllExercisesAsync();
	Task<ExerciseDto> GetExerciseByIdAsync(int id);
	Task<ExerciseDto> CreateExerciseAsync(CreateExerciseRequest request);
	Task UpdateExerciseAsync(int id, CreateExerciseRequest request);
	Task DeleteExerciseAsync(int id);
}

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

	public async Task<ExerciseDto> GetExerciseByIdAsync(int id)
	{
		var exercise = await _exerciseRepository.GetExerciseByIdAsync(id);
		if (exercise is null) throw new KeyNotFoundException($"Exercise with ID {id} not found.");

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

	public async Task UpdateExerciseAsync(int id, CreateExerciseRequest request)
	{
		var updated = new Exercise
		{
			Id = id,
			Name = request.Name,
			Equipment = request.Equipment,
			Target = request.Target
		};

		await _exerciseRepository.UpdateExerciseAsync(updated);
	}

	public async Task DeleteExerciseAsync(int id)
	{
		var existing = await _exerciseRepository.GetExerciseByIdAsync(id);

		if (existing is null) throw new KeyNotFoundException($"Exercise with ID {id} not found.");

		await _exerciseRepository.DeleteExerciseAsync(id);
	}
}