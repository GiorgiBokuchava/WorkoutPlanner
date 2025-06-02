using WorkoutPlanner.Contracts;
using WorkoutPlanner.Domain.Entities;
using WorkoutPlanner.Application.Interfaces.Repositories;
using WorkoutPlanner.Application.Interfaces.Services;

namespace WorkoutPlanner.Application.Services;

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

	public async Task<WorkoutExerciseDto?> GetWorkoutExerciseByIdAsync(int id)
	{
		var exercise = await _workoutExerciseRepository.GetWorkoutExerciseByIdAsync(id);
		if (exercise is null) return null;

		return new WorkoutExerciseDto(
			exercise.Id,
			exercise.WorkoutLogId,
			exercise.ExerciseId,
			exercise.SetsCompleted,
			exercise.RepsCompleted,
			exercise.WeightUsed
		);
	}

	public async Task<WorkoutExerciseDto> CreateWorkoutExerciseAsync(CreateWorkoutExerciseRequest request)
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

	public async Task<bool> UpdateWorkoutExerciseAsync(int id, UpdateWorkoutExerciseRequest request)
	{
		var existing = await _workoutExerciseRepository.GetWorkoutExerciseByIdAsync(id);
		if (existing is null) return false;

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
		return true;
	}

	public async Task<bool> DeleteWorkoutExerciseAsync(int id)
	{
		var existing = await _workoutExerciseRepository.GetWorkoutExerciseByIdAsync(id);
		if (existing is null) return false;

		await _workoutExerciseRepository.DeleteExerciseFromWorkoutLogAsync(existing.ExerciseId, existing.WorkoutLogId);
		return true;
	}
}