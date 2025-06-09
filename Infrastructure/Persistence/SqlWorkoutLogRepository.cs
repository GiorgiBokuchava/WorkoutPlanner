using Dapper;
using WorkoutPlanner.Domain.Entities;
using WorkoutPlanner.Infrastructure.Repositories;

namespace WorkoutPlanner.Infrastructure.Persistence;
public class SqlWorkoutLogRepository : RepositoryBase, IWorkoutLogRepository
{
	public SqlWorkoutLogRepository(IConfiguration configuration) : base(configuration) { }

	public async Task<IEnumerable<WorkoutLog>> GetAllWorkoutLogsAsync()
	{
		const string sql = @"
            SELECT
                id          AS Id,
                user_id     AS UserId,
                routine_id  AS RoutineId,
                log_date    AS Date,
                notes       AS Notes
            FROM [Identity].WorkoutLogs;
        ";
		using var db = GetConnection();
		return await db.QueryAsync<WorkoutLog>(sql);
	}

	public async Task<WorkoutLog?> GetWorkoutLogByIdAsync(int workoutLogId)
	{
		const string sql = @"
            SELECT
                id          AS Id,
                user_id     AS UserId,
                routine_id  AS RoutineId,
                log_date    AS Date,
                notes       AS Notes
            FROM [Identity].WorkoutLogs
            WHERE id = @Id;
        ";
		using var db = GetConnection();
		return await db.QuerySingleOrDefaultAsync<WorkoutLog>(sql, new { Id = workoutLogId });
	}

	public async Task<IEnumerable<WorkoutLog>> GetWorkoutLogsByUserIdAsync(int userId)
	{
		const string sql = @"
            SELECT
                id          AS Id,
                user_id     AS UserId,
                routine_id  AS RoutineId,
                log_date    AS Date,
                notes       AS Notes
            FROM [Identity].WorkoutLogs
            WHERE user_id = @UserId;
        ";
		using var db = GetConnection();
		return await db.QueryAsync<WorkoutLog>(sql, new { UserId = userId });
	}

	public async Task<int> AddWorkoutLogAsync(WorkoutLog workoutLog)
	{
		const string sql = @"
            INSERT INTO [Identity].WorkoutLogs 
                (user_id, routine_id, log_date, notes)
            OUTPUT INSERTED.id
            VALUES (@UserId, @RoutineId, @Date, @Notes);
        ";
		using var db = GetConnection();
		return await db.ExecuteScalarAsync<int>(sql, workoutLog);
	}

	public async Task UpdateWorkoutLogAsync(WorkoutLog workoutLog)
	{
		const string sql = @"
            UPDATE [Identity].WorkoutLogs
               SET routine_id = @RoutineId,
                   log_date   = @Date,
                   notes      = @Notes
             WHERE id = @Id;
        ";
		using var db = GetConnection();
		await db.ExecuteAsync(sql, workoutLog);
	}

	public async Task DeleteWorkoutLogAsync(int workoutLogId)
	{
		const string sql = @"
            DELETE FROM [Identity].WorkoutLogs
            WHERE id = @Id;
        ";
		using var db = GetConnection();
		await db.ExecuteAsync(sql, new { Id = workoutLogId });
	}

	public async Task<IEnumerable<WorkoutExercise>> GetAllWorkoutExercisesAsync()
	{
		const string sql = @"
            SELECT
                id              AS Id,
                workout_log_id  AS WorkoutLogId,
                exercise_id     AS ExerciseId,
                sets_completed  AS SetsCompleted,
                reps_completed  AS RepsCompleted,
                weight_used     AS WeightUsed
            FROM [Workout].WorkoutExercises;
        ";
		using var db = GetConnection();
		return await db.QueryAsync<WorkoutExercise>(sql);
	}

	public async Task<WorkoutExercise?> GetWorkoutExerciseByIdAsync(int workoutExerciseId)
	{
		const string sql = @"
            SELECT
                id              AS Id,
                workout_log_id  AS WorkoutLogId,
                exercise_id     AS ExerciseId,
                sets_completed  AS SetsCompleted,
                reps_completed  AS RepsCompleted,
                weight_used     AS WeightUsed
            FROM [Workout].WorkoutExercises
            WHERE id = @Id;
        ";
		using var db = GetConnection();
		return await db.QuerySingleOrDefaultAsync<WorkoutExercise>(sql, new { Id = workoutExerciseId });
	}

	public async Task<IEnumerable<WorkoutExercise>> GetExercisesByWorkoutLogIdAsync(int workoutLogId)
	{
		const string sql = @"
            SELECT
                id              AS Id,
                workout_log_id  AS WorkoutLogId,
                exercise_id     AS ExerciseId,
                sets_completed  AS SetsCompleted,
                reps_completed  AS RepsCompleted,
                weight_used     AS WeightUsed
            FROM [Workout].WorkoutExercises
            WHERE workout_log_id = @WorkoutLogId;
        ";
		using var db = GetConnection();
		return await db.QueryAsync<WorkoutExercise>(sql, new { WorkoutLogId = workoutLogId });
	}

	public async Task<int> AddExerciseToWorkoutLogAsync(WorkoutExercise workoutExercise)
	{
		const string sql = @"
            INSERT INTO [Workout].WorkoutExercises
                (workout_log_id, exercise_id, sets_completed, reps_completed, weight_used)
            OUTPUT INSERTED.id
            VALUES (@WorkoutLogId, @ExerciseId, @SetsCompleted, @RepsCompleted, @WeightUsed);
        ";
		using var db = GetConnection();
		return await db.ExecuteScalarAsync<int>(sql, workoutExercise);
	}

	public async Task UpdateExerciseInWorkoutLogAsync(WorkoutExercise workoutExercise)
	{
		const string sql = @"
            UPDATE [Workout].WorkoutExercises
               SET sets_completed = @SetsCompleted,
                   reps_completed = @RepsCompleted,
                   weight_used    = @WeightUsed
             WHERE id = @Id;
        ";
		using var db = GetConnection();
		await db.ExecuteAsync(sql, workoutExercise);
	}

	public async Task DeleteExerciseFromWorkoutLogAsync(int exerciseId, int workoutLogId)
	{
		const string sql = @"
            DELETE FROM [Workout].WorkoutExercises
            WHERE exercise_id    = @ExerciseId
              AND workout_log_id = @WorkoutLogId;
        ";
		using var db = GetConnection();
		await db.ExecuteAsync(sql, new { ExerciseId = exerciseId, WorkoutLogId = workoutLogId });
	}
}
