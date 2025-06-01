using WorkoutPlanner.Contracts;
using WorkoutPlanner.Domain.Entities;
using WorkoutPlanner.Application.Interfaces;

namespace WorkoutPlanner.Application.Services;

public interface IWorkoutExerciseService
{
	Task<IEnumerable<WorkoutExerciseDto>> GetAllWorkoutExercisesAsync();
	Task<WorkoutExerciseDto> GetWorkoutExerciseByIdAsync(int id);
	Task<WorkoutExerciseDto> CreateWorkoutExerciseAsync(CreateWorkoutExerciseRequest request);
	Task UpdateWorkoutExerciseAsync(int id, CreateWorkoutExerciseRequest request);
	Task DeleteWorkoutExerciseAsync(int id);
}

public class WorkoutExerciseService : IWorkoutExerciseService
{
	private readonly IWorkoutLogRepository _workoutExerciseRepository;

	public WorkoutExerciseService(IWorkoutLogRepository workoutExerciseRepository)
	{
		_workoutExerciseRepository = workoutExerciseRepository;
	}

	public async Task<IEnumerable<WorkoutExerciseDto>> GetAllWorkoutExercisesAsync()
	{
		var exercises = await _workoutExerciseRepository.GetAllWorkoutExercisesAsync();
		return exercises.Select(e => new WorkoutExerciseDto(
			e.Id,
			e.WorkoutLogId,
			e.ExerciseId,
			e.SetsCompleted,
			e.RepsCompleted,
			e.WeightUsed
		));
	}
	public async Task<WorkoutExerciseDto> GetWorkoutExerciseByIdAsync(int id)
	{
		var exercise = await _workoutExerciseRepository.GetWorkoutExerciseByIdAsync(id);
		if (exercise is null) throw new
				KeyNotFoundException($"Workout exercise with ID {id} not found.");

		return new WorkoutExerciseDto(
				exercise.Id,
				exercise.WorkoutLogId,
				exercise.ExerciseId,
				exercise.SetsCompleted,
				exercise.RepsCompleted,
				exercise.WeightUsed
			);
	}
	public async Task<WorkoutExerciseDto>
		CreateWorkoutExerciseAsync(CreateWorkoutExerciseRequest request)
	{
		var workoutExercise = new WorkoutExercise
		{
			WorkoutLogId = request.WorkoutLogId,
			ExerciseId = request.ExerciseId,
			SetsCompleted = request.SetsCompleted,
			RepsCompleted = request.RepsCompleted,
			WeightUsed = request.WeightUsed
		};
		workoutExercise.Id = await _workoutExerciseRepository
			.AddExerciseToWorkoutLogAsync(workoutExercise);

		return new WorkoutExerciseDto(
			workoutExercise.Id,
			workoutExercise.WorkoutLogId,
			workoutExercise.ExerciseId,
			workoutExercise.SetsCompleted,
			workoutExercise.RepsCompleted,
			workoutExercise.WeightUsed
		);
	}
	public async Task UpdateWorkoutExerciseAsync(int id, CreateWorkoutExerciseRequest request)
	{
		var existing = await _workoutExerciseRepository.GetWorkoutExerciseByIdAsync(id)
			?? throw new KeyNotFoundException($"Workout exercise with ID {id} not found.");

		var updatedExercise = new WorkoutExercise
		{
			Id = id,
			WorkoutLogId = request.WorkoutLogId,
			ExerciseId = request.ExerciseId,
			SetsCompleted = request.SetsCompleted,
			RepsCompleted = request.RepsCompleted,
			WeightUsed = request.WeightUsed
		};

		await _workoutExerciseRepository.UpdateExerciseInWorkoutLogAsync(updatedExercise);
	}

	public async Task DeleteWorkoutExerciseAsync(int id)
	{
		var existing = await _workoutExerciseRepository.GetWorkoutExerciseByIdAsync(id)
			?? throw new KeyNotFoundException($"Workout exercise with ID {id} not found.");

		await _workoutExerciseRepository.
			DeleteExerciseFromWorkoutLogAsync(existing.ExerciseId, existing.WorkoutLogId);
	}
}