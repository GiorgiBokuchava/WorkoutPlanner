namespace WorkoutPlanner_API.Models
{
    public class Routine
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int FrequencyPerWeek { get; set; }
        public string Difficulty { get; set; } = string.Empty;
    }
}
