namespace WorkoutPlanner_API.Models
{
    public class WorkoutExercise
    {
        public int Id { get; set; }

        public int WorkoutLogId { get; set; }
        public WorkoutLog WorkoutLog { get; set; } = null!;

        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; } = null!;

        public int SetsCompleted { get; set; }
        public int RepsCompleted { get; set; }
        public decimal? WeightUsed { get; set; }
    }
}