using WorkoutPlanner.Application.Interfaces;
using WorkoutPlanner.Models;

namespace WorkoutPlanner.Infrastructure.Persistance;
public class SqlExerciseRepository : IExerciseRepository
{
	private readonly string _connectinString;

	public SqlExerciseRepository(IConfiguration configuration)
	{
		_connectinString = configuration.GetConnectionString("DefaultConnection")!;
	}

	public Task AddExerciseAsync(Exercise exercise)
	{
		throw new NotImplementedException();
	}

	public Task DeleteExerciseAsync(int exerciseId)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<Exercise>> GetAllExercisesAsync()
	{
		throw new NotImplementedException();
	}

	public Task<Exercise?> GetExerciseByIdAsync(int exerciseId)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<Exercise>> GetExercisesByEquipmentAsync(string equipment)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<Exercise>> GetExercisesByTargetAsync(string target)
	{
		throw new NotImplementedException();
	}

	public Task UpdateExerciseAsync(Exercise exercise)
	{
		throw new NotImplementedException();
	}
}
