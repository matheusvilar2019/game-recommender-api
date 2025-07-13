using System.Text.Json.Serialization;

namespace GameRecommenderAPI.Dtos
{
    public class GameDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("freetogame_profile_url")]
        public string FreetogameProfileUrl { get; set; }

        [JsonPropertyName("genre")]
        public string Genre { get; set; }
    }
}
