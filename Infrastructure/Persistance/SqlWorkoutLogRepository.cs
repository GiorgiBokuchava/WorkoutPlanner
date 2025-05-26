using WorkoutPlanner_API.Application.Interfaces;
using WorkoutPlanner_API.Models;

namespace WorkoutPlanner_API.Infrastructure.Persistance
{
	public class SqlWorkoutLogRepository : IWorkoutLogRepository
	{
		private readonly string _connectionString;

		public SqlWorkoutLogRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection")!;
		}

		public Task AddExerciseToWorkoutLogAsync(WorkoutExercise workoutExercise)
		{
			throw new NotImplementedException();
		}

		public Task AddWorkoutLogAsync(WorkoutLog workoutLog)
		{
			throw new NotImplementedException();
		}

		public Task DeleteExerciseFromWorkoutLogAsync(int exerciseId, int workoutLogId)
		{
			throw new NotImplementedException();
		}

		public Task DeleteWorkoutLogAsync(int workoutLogId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<WorkoutExercise>> GetExercisesByWorkoutLogIdAsync(int workoutLogId)
		{
			throw new NotImplementedException();
		}

		public Task<WorkoutLog?> GetWorkoutLogByIdAsync(int workoutLogId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<WorkoutLog>> GetWorkoutLogsByUserIdAsync(int userId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<WorkoutLog>> GetAllWorkoutLogsAsync()
		{
			throw new NotImplementedException();
		}


		public Task UpdateExerciseInWorkoutLogAsync(WorkoutExercise workoutExercise)
		{
			throw new NotImplementedException();
		}

		public Task UpdateWorkoutLogAsync(WorkoutLog workoutLog)
		{
			throw new NotImplementedException();
		}
	}
}