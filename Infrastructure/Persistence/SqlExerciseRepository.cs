using Dapper;
using WorkoutPlanner.Domain.Entities;
using WorkoutPlanner.Infrastructure.Repositories;

namespace WorkoutPlanner.Infrastructure.Persistence;
public class SqlExerciseRepository : RepositoryBase, IExerciseRepository
{
	public SqlExerciseRepository(IConfiguration configuration) : base(configuration) { }

	public async Task<int> AddExerciseAsync(Exercise exercise)
	{
		const string sql = @"
			INSERT INTO [Workout].Exercises (Name, Equipment, Target)
			OUTPUT INSERTED.Id
			VALUES (@Name, @Equipment, @Target);";

		using var db = GetConnection();
		return await db.ExecuteScalarAsync<int>(sql, exercise);
	}

	public async Task DeleteExerciseAsync(int exerciseId)
	{
		const string sql = @"
			DELETE FROM [Workout].Exercises
			WHERE Id = @Id";

		using var db = GetConnection();
		await db.ExecuteAsync(sql, new { Id = exerciseId });
	}

	public async Task<IEnumerable<Exercise>> GetAllExercisesAsync()
	{
		const string sql = "SELECT Id, Name, Equipment, Target FROM [Workout].Exercises;";

		using var db = GetConnection();
		return await db.QueryAsync<Exercise>(sql);
	}

	public async Task<Exercise?> GetExerciseByIdAsync(int exerciseId)
	{
		const string sql = "SELECT Id, Name, Equipment, Target FROM [Workout].Exercises WHERE Id = @Id";

		using var db = GetConnection();
		return await db.QuerySingleOrDefaultAsync<Exercise>(sql, new { Id = exerciseId });
	}

	public async Task<IEnumerable<Exercise>> GetExercisesByEquipmentAsync(string equipment)
	{
		const string sql = "SELECT Id, Name, Equipment, Target FROM [Workout].Exercises WHERE Equipment = @Equipment";

		using var db = GetConnection();
		return await db.QueryAsync<Exercise>(sql, new { Equipment = equipment });
	}

	public async Task<IEnumerable<Exercise>> GetExercisesByTargetAsync(string target)
	{
		const string sql = "SELECT Id, Name, Equipment, Target FROM [Workout].Exercises WHERE Target = @Target";

		using var db = GetConnection();
		return await db.QueryAsync<Exercise>(sql, new { Target = target });
	}

	public async Task UpdateExerciseAsync(Exercise exercise)
	{
		const string sql = @"
			UPDATE [Workout].Exercises
			SET Name = @Name, Equipment = @Equipment, Target = @Target
			WHERE Id = @Id";

		using var db = GetConnection();
		await db.ExecuteAsync(sql, exercise);
	}
}
