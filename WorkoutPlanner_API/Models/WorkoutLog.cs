namespace WorkoutPlanner_API.Models
{
    public class WorkoutLog
    {
        public int Id { get; set; }
        public int RoutineId { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
