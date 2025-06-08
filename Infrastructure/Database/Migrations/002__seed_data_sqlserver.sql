USE [WorkoutPlanner-db];
GO

-- Seed Identity.Users
IF NOT EXISTS (SELECT 1 FROM [Identity].Users WHERE email = 'alice@example.com')
BEGIN
    INSERT INTO [Identity].Users (name, email, password_hash)
    VALUES (
        'Alice',
        'alice@example.com',
        '$2a$10$E/7aKcZ2dQ6n4Q11K0s1euKQ7vD2b5n0GkXkEpPZb86.m3HGDZ0oG'
    );
END

IF NOT EXISTS (SELECT 1 FROM [Identity].Users WHERE email = 'bob@example.com')
BEGIN
    INSERT INTO [Identity].Users (name, email, password_hash)
    VALUES (
        'Bob',
        'bob@example.com',
        '$2a$10$pV1a3FAfG.jKxmrLKpK1jeLSoijGH8XYRkFq6sGdUcZSx0HVY/2Ge'
    );
END

DECLARE @AliceId int  = (SELECT TOP 1 id FROM [Identity].Users WHERE email = 'alice@example.com' ORDER BY id);
DECLARE @BobId   int  = (SELECT TOP 1 id FROM [Identity].Users WHERE email = 'bob@example.com' ORDER BY id);

-- Seed Identity.Routines
IF NOT EXISTS (SELECT 1 FROM [Identity].Routines
               WHERE user_id = @AliceId AND title = 'Push-Pull-Legs')
BEGIN
    INSERT INTO [Identity].Routines (user_id, title, frequency_per_week, difficulty)
    VALUES (@AliceId, 'Push-Pull-Legs', 3, 'Medium');
END

IF NOT EXISTS (SELECT 1 FROM [Identity].Routines
               WHERE user_id = @BobId AND title = 'Full Body Blast')
BEGIN
    INSERT INTO [Identity].Routines (user_id, title, frequency_per_week, difficulty)
    VALUES (@BobId, 'Full Body Blast', 4, 'Hard');
END

DECLARE @PPLId  int = (SELECT TOP 1 id FROM [Identity].Routines
                       WHERE user_id = @AliceId AND title = 'Push-Pull-Legs' ORDER BY id);
DECLARE @FBBId  int = (SELECT TOP 1 id FROM [Identity].Routines
                       WHERE user_id = @BobId   AND title = 'Full Body Blast' ORDER BY id);

-- Seed Workout.Exercises
IF NOT EXISTS (SELECT 1 FROM [Workout].Exercises WHERE name = 'Bench Press')
    INSERT INTO [Workout].Exercises (name, equipment, target)
    VALUES ('Bench Press', 'Barbell', 'Chest');

IF NOT EXISTS (SELECT 1 FROM [Workout].Exercises WHERE name = 'Squat')
    INSERT INTO [Workout].Exercises (name, equipment, target)
    VALUES ('Squat', 'Barbell', 'Legs');

DECLARE @BenchId int = (SELECT TOP 1 id FROM [Workout].Exercises WHERE name = 'Bench Press' ORDER BY id);
DECLARE @SquatId int = (SELECT TOP 1 id FROM [Workout].Exercises WHERE name = 'Squat' ORDER BY id);

-- Seed Workout.RoutineExercises
IF NOT EXISTS (SELECT 1 FROM [Workout].RoutineExercises
               WHERE routine_id = @PPLId AND exercise_id = @BenchId)
BEGIN
    INSERT INTO [Workout].RoutineExercises
          (routine_id, exercise_id, sets, reps_per_set, weight)
    VALUES (@PPLId, @BenchId, 4, 8, 100.00);
END

IF NOT EXISTS (SELECT 1 FROM [Workout].RoutineExercises
               WHERE routine_id = @PPLId AND exercise_id = @SquatId)
BEGIN
    INSERT INTO [Workout].RoutineExercises
          (routine_id, exercise_id, sets, reps_per_set, weight)
    VALUES (@PPLId, @SquatId, 3, 5, 150.00);
END

-- Seed Identity.WorkoutLogs
IF NOT EXISTS (SELECT 1 FROM [Identity].WorkoutLogs
               WHERE user_id = @AliceId AND log_date = '2025-05-25')
BEGIN
    INSERT INTO [Identity].WorkoutLogs (user_id, routine_id, log_date, notes)
    VALUES (@AliceId, @PPLId, '2025-05-25', 'Felt strong');
END
IF NOT EXISTS (SELECT 1 FROM [Identity].WorkoutLogs
               WHERE user_id = @BobId AND log_date = '2025-05-25')
BEGIN
    INSERT INTO [Identity].WorkoutLogs (user_id, routine_id, log_date, notes)
    VALUES (@BobId,   @FBBId, '2025-05-25', 'Great session');
END

DECLARE @AliceLogId int = (SELECT TOP 1 id FROM [Identity].WorkoutLogs
                            WHERE user_id = @AliceId AND log_date = '2025-05-25' ORDER BY id);
DECLARE @BobLogId   int = (SELECT TOP 1 id FROM [Identity].WorkoutLogs
                            WHERE user_id = @BobId   AND log_date = '2025-05-25' ORDER BY id);

-- Seed Workout.WorkoutExercises
IF NOT EXISTS (SELECT 1 FROM [Workout].WorkoutExercises
               WHERE workout_log_id = @AliceLogId AND exercise_id = @BenchId)
BEGIN
    INSERT INTO [Workout].WorkoutExercises
          (workout_log_id, exercise_id, sets_completed, reps_completed, weight_used)
    VALUES (@AliceLogId, @BenchId, 4, 8, 100.00);
END

IF NOT EXISTS (SELECT 1 FROM [Workout].WorkoutExercises
               WHERE workout_log_id = @BobLogId AND exercise_id = @SquatId)
BEGIN
    INSERT INTO [Workout].WorkoutExercises
          (workout_log_id, exercise_id, sets_completed, reps_completed, weight_used)
    VALUES (@BobLogId, @SquatId, 3, 5, 150.00);
END
GO

-- Cascade‐delete from RoutineExercises -> Exercises
ALTER TABLE [Workout].RoutineExercises
DROP CONSTRAINT FK_RtEx_Exercise;

ALTER TABLE [Workout].RoutineExercises
ADD CONSTRAINT FK_RtEx_Exercise
  FOREIGN KEY (exercise_id)
  REFERENCES [Workout].Exercises(id)
  ON DELETE CASCADE;

-- Cascade‐delete from WorkoutExercises -> Exercises
ALTER TABLE [Workout].WorkoutExercises
DROP CONSTRAINT FK_WE_Exercise;

ALTER TABLE [Workout].WorkoutExercises
ADD CONSTRAINT FK_WE_Exercise
  FOREIGN KEY (exercise_id)
  REFERENCES [Workout].Exercises(id)
  ON DELETE CASCADE;