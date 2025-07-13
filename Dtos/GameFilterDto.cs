namespace GameRecommenderAPI.Dtos
{
    public class GameFilterDto
    {
        public string Category { get; set; } = string.Empty;
        public string? Platform { get; set; }
        public int? RamMemory { get; set; }
    }
}
