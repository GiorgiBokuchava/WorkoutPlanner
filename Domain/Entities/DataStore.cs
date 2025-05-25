namespace WorkoutPlanner_API.Domain.Entities
{
    public static class DataStore
    {
        public static List<User> Users { get; } = new();
        public static List<Exercise> Exercises { get; } = new();
        public static List<Routine> Routines { get; } = new();
        public static List<WorkoutLog> WorkoutLogs { get; } = new();

        // for new junction tables:
        public static List<RoutineExercise> RoutineExercises { get; } = new();
        public static List<WorkoutExercise> WorkoutExercises { get; } = new();
    }
}
