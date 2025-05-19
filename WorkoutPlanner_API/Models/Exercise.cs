namespace WorkoutPlanner_API.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
        public string Equipment { get; set; } = string.Empty;
    }
}
