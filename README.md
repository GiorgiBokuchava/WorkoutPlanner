# Workout Planner API

A .NET 9.0 Web API for managing workout routines, exercises, and tracking workout sessions.

## Features

- User management with basic CRUD operations
- Exercise catalog management
- Workout routine creation and management
- Workout logging and tracking
- Swagger UI for API documentation and testing

## Prerequisites

- .NET 9.0 SDK
- SQL Server Express
- Visual Studio 2022 or a compatible IDE

## Installation

1. **Clone the repository:**
git clone <repository-url> cd workout-planner-api

2. **Setup the database:**
sqlcmd -S .\SQLEXPRESS -E -i database\01_create_tables.sql
sqlcmd -S .\SQLEXPRESS -E -i database\02_seed_data.sql

3. **Start the application:**
dotnet run

The API will be available at `https://localhost:5001` and the Swagger UI at `https://localhost:5001/swagger`.

## API Endpoints

### Users
- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create a new user
- `PUT /api/users/{id}` - Update user details
- `DELETE /api/users/{id}` - Delete a user

### Exercises
- `GET /api/exercises` - Get all exercises
- `GET /api/exercises/{id}` - Get exercise by ID
- `POST /api/exercises` - Create a new exercise
- `PUT /api/exercises/{id}` - Update exercise details
- `DELETE /api/exercises/{id}` - Delete an exercise

### Workout Logs
- `GET /api/workoutLogs` - Get all workout logs
- `GET /api/workoutLogs/{id}` - Get a specific workout log by ID
- `GET /api/workoutLogs/users/{userId}` - Get workout logs for a specific user
- `POST /api/workoutLogs?userId={userId}` - Create a new workout log
- `PUT /api/workoutLogs/{id}?userId={userId}` - Update a workout log
- `DELETE /api/workoutLogs/{id}` - Delete a workout log

## Data Models

### User
- **Id** (int): Unique identifier
- **Name** (string): User's name
- **Email** (string): User's email address
- **PasswordHash** (string): User's hashed password

### Exercise
- **Id** (int): Unique identifier
- **Name** (string): Exercise name
- **Equipment** (string): Required equipment for the exercise
- **Target** (string): Target muscle group

### WorkoutLog
- **Id** (int): Unique identifier
- **UserId** (int): ID of the user who performed the workout
- **RoutineId** (int): ID of the performed routine
- **Date** (DateTime): Date and time of the workout
- **Notes** (string): Optional workout notes
- **WorkoutExercises** (Collection): Exercises performed during the workout

## Technology Stack

- .NET 9.0
- ASP.NET Core Web API
- SQL Server Express
- Swagger/OpenAPI (Swashbuckle.AspNetCore)
- Microsoft.Extensions.Configuration.Json

## Architecture

For an overview of the system architecture, review the [DB Diagram](https://dbdiagram.io/d/WorkoutPlanner-682cc88eb9f7446da3618091) which visualizes the relationships between the key data models in this project.

## Development

The API follows RESTful routing conventions and returns JSON responses. Swagger UI is available for API testing and documentation when running the application in development mode.

## License

[MIT License](LICENSE)