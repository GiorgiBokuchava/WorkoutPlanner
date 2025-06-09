using Dapper;
using WorkoutPlanner.Domain.Entities;
using WorkoutPlanner.Infrastructure.Repositories;

namespace WorkoutPlanner.Infrastructure.Persistence;
public class SqlRoutineRepository : RepositoryBase, IRoutineRepository
{
	public SqlRoutineRepository(IConfiguration configuration) : base(configuration) { }

	public async Task<IEnumerable<Routine>> GetAllRoutinesAsync()
	{
		const string sql = @"
            SELECT
                id                  AS Id,
                user_id             AS UserId,
                title               AS Title,
                frequency_per_week  AS FrequencyPerWeek,
                difficulty          AS Difficulty
            FROM [Identity].Routines;
        ";
		using var db = GetConnection();
		return await db.QueryAsync<Routine>(sql);
	}

	public async Task<Routine?> GetRoutineByIdAsync(int routineId)
	{
		const string sql = @"
            SELECT
                id                  AS Id,
                user_id             AS UserId,
                title               AS Title,
                frequency_per_week  AS FrequencyPerWeek,
                difficulty          AS Difficulty
            FROM [Identity].Routines
            WHERE id = @Id;
        ";
		using var db = GetConnection();
		return await db.QuerySingleOrDefaultAsync<Routine>(sql, new { Id = routineId });
	}

	public async Task<IEnumerable<Routine>> GetRoutinesByUserIdAsync(int userId)
	{
		const string sql = @"
            SELECT
                id                  AS Id,
                user_id             AS UserId,
                title               AS Title,
                frequency_per_week  AS FrequencyPerWeek,
                difficulty          AS Difficulty
            FROM [Identity].Routines
            WHERE user_id = @UserId;
        ";
		using var db = GetConnection();
		return await db.QueryAsync<Routine>(sql, new { UserId = userId });
	}

	public async Task<int> AddRoutineAsync(Routine routine)
	{
		const string sql = @"
            INSERT INTO [Identity].Routines
                (user_id, title, frequency_per_week, difficulty)
            OUTPUT INSERTED.id
            VALUES (@UserId, @Title, @FrequencyPerWeek, @Difficulty);
        ";
		using var db = GetConnection();
		return await db.ExecuteScalarAsync<int>(sql, routine);
	}

	public async Task UpdateRoutineAsync(Routine routine)
	{
		const string sql = @"
            UPDATE [Identity].Routines
               SET title               = @Title,
                   frequency_per_week  = @FrequencyPerWeek,
                   difficulty          = @Difficulty
             WHERE id = @Id;
        ";
		using var db = GetConnection();
		await db.ExecuteAsync(sql, routine);
	}

	public async Task DeleteRoutineAsync(int routineId)
	{
		const string sql = @"
            DELETE FROM [Identity].Routines
            WHERE id = @Id;
        ";
		using var db = GetConnection();
		await db.ExecuteAsync(sql, new { Id = routineId });
	}

	public async Task<IEnumerable<RoutineExercise>> GetAllRoutineExercisesAsync()
	{
		const string sql = @"
            SELECT
                id             AS Id,
                routine_id     AS RoutineId,
                exercise_id    AS ExerciseId,
                sets           AS Sets,
                reps_per_set   AS RepsPerSet,
                weight         AS Weight
            FROM [Workout].RoutineExercises;
        ";
		using var db = GetConnection();
		return await db.QueryAsync<RoutineExercise>(sql);
	}

	public async Task<RoutineExercise?> GetRoutineExerciseByIdAsync(int routineExerciseId)
	{
		const string sql = @"
            SELECT
                id             AS Id,
                routine_id     AS RoutineId,
                exercise_id    AS ExerciseId,
                sets           AS Sets,
                reps_per_set   AS RepsPerSet,
                weight         AS Weight
            FROM [Workout].RoutineExercises
            WHERE id = @Id;
        ";
		using var db = GetConnection();
		return await db.QuerySingleOrDefaultAsync<RoutineExercise>(sql, new { Id = routineExerciseId });
	}

	public async Task<IEnumerable<RoutineExercise>> GetExercisesByRoutineIdAsync(int routineId)
	{
		const string sql = @"
            SELECT
                id             AS Id,
                routine_id     AS RoutineId,
                exercise_id    AS ExerciseId,
                sets           AS Sets,
                reps_per_set   AS RepsPerSet,
                weight         AS Weight
            FROM [Workout].RoutineExercises
            WHERE routine_id = @RoutineId;
        ";
		using var db = GetConnection();
		return await db.QueryAsync<RoutineExercise>(sql, new { RoutineId = routineId });
	}

	public async Task<int> AddExerciseToRoutineAsync(RoutineExercise routineExercise)
	{
		const string sql = @"
            INSERT INTO [Workout].RoutineExercises
                (routine_id, exercise_id, sets, reps_per_set, weight)
            OUTPUT INSERTED.id
            VALUES (@RoutineId, @ExerciseId, @Sets, @RepsPerSet, @Weight);
        ";
		using var db = GetConnection();
		return await db.ExecuteScalarAsync<int>(sql, routineExercise);
	}

	public async Task UpdateExerciseInRoutineAsync(RoutineExercise routineExercise)
	{
		const string sql = @"
            UPDATE [Workout].RoutineExercises
               SET exercise_id  = @ExerciseId,
                   sets         = @Sets,
                   reps_per_set = @RepsPerSet,
                   weight       = @Weight
             WHERE id = @Id;
        ";
		using var db = GetConnection();
		await db.ExecuteAsync(sql, routineExercise);
	}

	public async Task DeleteExerciseFromRoutineAsync(int exerciseId, int routineId)
	{
		const string sql = @"
            DELETE FROM [Workout].RoutineExercises
            WHERE exercise_id = @ExerciseId
              AND routine_id  = @RoutineId;
        ";
		using var db = GetConnection();
		await db.ExecuteAsync(sql, new { ExerciseId = exerciseId, RoutineId = routineId });
	}
}
