USE master;
GO

-- Create database if it doesn't exist
IF DB_ID('WorkoutPlanner-db') IS NULL
  CREATE DATABASE [WorkoutPlanner-db];
GO

-- Switch into the project database
USE [WorkoutPlanner-db];
GO

-- Create schemas if missing
IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Identity')
  EXEC('CREATE SCHEMA [Identity]');
GO
IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Workout')
  EXEC('CREATE SCHEMA [Workout]');
GO

-- Identity.Users
IF OBJECT_ID('Identity.Users','U') IS NULL
CREATE TABLE [Identity].Users (
  id INT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(100) NOT NULL,
  email VARCHAR(200) NOT NULL UNIQUE,
  password_hash VARCHAR(256) NOT NULL
);
GO

-- Identity.Routines
IF OBJECT_ID('Identity.Routines','U') IS NULL
CREATE TABLE [Identity].Routines (
  id INT IDENTITY(1,1) PRIMARY KEY,
  user_id INT NOT NULL
    CONSTRAINT FK_Routines_Users FOREIGN KEY REFERENCES [Identity].Users(id),
  title VARCHAR(150) NOT NULL,
  frequency_per_week INT NOT NULL,
  difficulty VARCHAR(50) NOT NULL
);
GO
IF NOT EXISTS (
  SELECT 1 FROM sys.indexes 
  WHERE name = 'idx_routines_user_id' AND object_id = OBJECT_ID('Identity.Routines')
)
  CREATE INDEX idx_routines_user_id ON [Identity].Routines(user_id);
GO

-- Identity.WorkoutLogs
IF OBJECT_ID('Identity.WorkoutLogs','U') IS NULL
CREATE TABLE [Identity].WorkoutLogs (
  id INT IDENTITY(1,1) PRIMARY KEY,
  user_id INT NOT NULL
    CONSTRAINT FK_WorkoutLogs_Users FOREIGN KEY REFERENCES [Identity].Users(id),
  routine_id INT NOT NULL
    CONSTRAINT FK_WorkoutLogs_Routines FOREIGN KEY REFERENCES [Identity].Routines(id),
  log_date DATETIME NOT NULL,
  notes VARCHAR(500) NULL
);
GO
IF NOT EXISTS (
  SELECT 1 FROM sys.indexes 
  WHERE name = 'idx_workoutlogs_user_id' AND object_id = OBJECT_ID('Identity.WorkoutLogs')
)
  CREATE INDEX idx_workoutlogs_user_id ON [Identity].WorkoutLogs(user_id);
GO
IF NOT EXISTS (
  SELECT 1 FROM sys.indexes 
  WHERE name = 'idx_workoutlogs_routine_id' AND object_id = OBJECT_ID('Identity.WorkoutLogs')
)
  CREATE INDEX idx_workoutlogs_routine_id ON [Identity].WorkoutLogs(routine_id);
GO

-- Workout.Exercises
IF OBJECT_ID('Workout.Exercises','U') IS NULL
CREATE TABLE [Workout].Exercises (
  id INT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(100) NOT NULL,
  equipment VARCHAR(100) NULL,
  target VARCHAR(100) NULL
);
GO

-- Workout.RoutineExercises
IF OBJECT_ID('Workout.RoutineExercises','U') IS NULL
CREATE TABLE [Workout].RoutineExercises (
  id INT IDENTITY(1,1) PRIMARY KEY,
  routine_id INT NOT NULL
    CONSTRAINT FK_RtEx_Routine FOREIGN KEY REFERENCES [Identity].Routines(id),
  exercise_id INT NOT NULL
    CONSTRAINT FK_RtEx_Exercise FOREIGN KEY REFERENCES [Workout].Exercises(id),
  sets INT NOT NULL,
  reps_per_set INT NOT NULL,
  weight DECIMAL(7,2) NULL
);
GO
IF NOT EXISTS (
  SELECT 1 FROM sys.indexes 
  WHERE name = 'idx_rtex_routine_id' AND object_id = OBJECT_ID('Workout.RoutineExercises')
)
  CREATE INDEX idx_rtex_routine_id ON [Workout].RoutineExercises(routine_id);
GO
IF NOT EXISTS (
  SELECT 1 FROM sys.indexes 
  WHERE name = 'idx_rtex_exercise_id' AND object_id = OBJECT_ID('Workout.RoutineExercises')
)
  CREATE INDEX idx_rtex_exercise_id ON [Workout].RoutineExercises(exercise_id);
GO

-- Workout.WorkoutExercises
IF OBJECT_ID('Workout.WorkoutExercises','U') IS NULL
CREATE TABLE [Workout].WorkoutExercises (
  id INT IDENTITY(1,1) PRIMARY KEY,
  workout_log_id INT NOT NULL
    CONSTRAINT FK_WE_Log FOREIGN KEY REFERENCES [Identity].WorkoutLogs(id),
  exercise_id INT NOT NULL
    CONSTRAINT FK_WE_Exercise FOREIGN KEY REFERENCES [Workout].Exercises(id),
  sets_completed INT NOT NULL,
  reps_completed INT NOT NULL,
  weight_used DECIMAL(7,2) NULL
);
GO
IF NOT EXISTS (
  SELECT 1 FROM sys.indexes 
  WHERE name = 'idx_wtex_log_id' AND object_id = OBJECT_ID('Workout.WorkoutExercises')
)
  CREATE INDEX idx_wtex_log_id ON [Workout].WorkoutExercises(workout_log_id);
GO
IF NOT EXISTS (
  SELECT 1 FROM sys.indexes 
  WHERE name = 'idx_wtex_exercise_id' AND object_id = OBJECT_ID('Workout.WorkoutExercises')
)
  CREATE INDEX idx_wtex_exercise_id ON [Workout].WorkoutExercises(exercise_id);
GO