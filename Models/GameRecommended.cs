namespace GameRecommenderAPI.Models
{
    public class GameRecommended
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public int Counter { get; set; } = 1;
    }
}
