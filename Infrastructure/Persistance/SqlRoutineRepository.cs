using Application.Interfaces;
using WorkoutPlanner_API.Application.Interfaces;
using WorkoutPlanner_API.Domain.Entities;

namespace WorkoutPlanner_API.Infrastructure.Persistance
{
	public class SqlRoutineRepository : IRoutineRepository
	{
		public readonly string _connectionString;

		public SqlRoutineRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection")!;
		}

		public Task AddExerciseToRoutineAsync(RoutineExercise routineExercise)
		{
			throw new NotImplementedException();
		}

		public Task AddRoutineAsync(Routine routine)
		{
			throw new NotImplementedException();
		}

		public Task DeleteExerciseFromRoutineAsync(int exerciseId, int routineId)
		{
			throw new NotImplementedException();
		}

		public Task DeleteRoutineAsync(int routineId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<RoutineExercise>> GetExercisesByRoutineIdAsync(int routineId)
		{
			throw new NotImplementedException();
		}

		public Task<Routine?> GetRoutineByIdAsync(int routineId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Routine>> GetRoutinesByUserIdAsync(int userId)
		{
			throw new NotImplementedException();
		}

		public Task UpdateExerciseInRoutineAsync(RoutineExercise routineExercise)
		{
			throw new NotImplementedException();
		}

		public Task UpdateRoutineAsync(Routine routine)
		{
			throw new NotImplementedException();
		}
	}
}
