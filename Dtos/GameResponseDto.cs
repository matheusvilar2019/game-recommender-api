using System.Text.Json.Serialization;

namespace GameRecommenderAPI.Dtos
{
    public class GameResponseDto
    {
        public string Title { get; set; }
        public string FreetogameProfileUrl { get; set; }
    }
}
