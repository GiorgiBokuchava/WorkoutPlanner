using WorkoutPlanner.Domain.Entities;

using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using WorkoutPlanner.Application.Interfaces.Repositories;

namespace WorkoutPlanner.Infrastructure.Persistance;
public class SqlExerciseRepository : IExerciseRepository
{
	private readonly IConfiguration _configuration;

	public SqlExerciseRepository(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	private IDbConnection GetConnection()
	{
		return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"))
			?? throw new InvalidOperationException("DefaultConnection is not set");
	}

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
