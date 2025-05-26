USE [WorkoutPlanner-db];
GO

-- Seed Identity.Users
INSERT INTO [Identity].Users (name, email, password_hash)
VALUES
  ('Alice', 'alice@example.com', 'hash1'),
  ('Bob', 'bob@example.com', 'hash2');
GO

-- Seed Identity.Routines
INSERT INTO [Identity].Routines (user_id, title, frequency_per_week, difficulty)
VALUES
  (1, 'Push-Pull-Legs', 3, 'Medium'),
  (2, 'Full Body Blast', 4, 'Hard');
GO

-- Seed Workout.Exercises
INSERT INTO [Workout].Exercises (name, equipment, target)
VALUES
  ('Bench Press', 'Barbell', 'Chest'),
  ('Squat', 'Barbell', 'Legs');
GO

-- Seed Workout.RoutineExercises
INSERT INTO [Workout].RoutineExercises (routine_id, exercise_id, sets, reps_per_set, weight)
VALUES
  (1, 1, 4, 8, 100.00),
  (1, 2, 3, 5, 150.00);
GO

-- Seed Identity.WorkoutLogs
INSERT INTO [Identity].WorkoutLogs (user_id, routine_id, log_date, notes)
VALUES
  (1, 1, '2025-05-25', 'Felt strong'),
  (2, 2, '2025-05-25', 'Great session');
GO

-- Seed Workout.WorkoutExercises
INSERT INTO [Workout].WorkoutExercises (workout_log_id, exercise_id, sets_completed, reps_completed, weight_used)
VALUES
  (1, 1, 4, 8, 100.00),
  (2, 2, 3, 5, 150.00);
GO
