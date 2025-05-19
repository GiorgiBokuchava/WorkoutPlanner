using WorkoutPlanner_API.Models;

namespace WorkoutPlanner_API.Data;

public static class DataStore
{
    public static List<User> Users { get; } = new();
    public static List<Exercise> Exercises { get; } = new();
    public static List<Routine> Routines { get; } = new();
    public static List<WorkoutLog> WorkoutLogs { get; } = new();
}