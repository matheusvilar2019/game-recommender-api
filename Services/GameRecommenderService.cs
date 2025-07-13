using GameRecommenderAPI.Data;
using GameRecommenderAPI.Dtos;
using GameRecommenderAPI.Exceptions;
using GameRecommenderAPI.Models;
using GameRecommenderAPI.Services.Requirements;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace GameRecommenderAPI.Services
{
    public class GameRecommenderService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRequirementExtractor _requirementExtractor;
        private readonly GameRecommenderDataContext _context;
        private readonly Random _random;
        public GameRecommenderService(IHttpClientFactory httpClientFactory, 
                                      GameRecommenderDataContext context, 
                                      IRequirementExtractor requirementExtractor) 
        {
            _httpClientFactory = httpClientFactory;
            _requirementExtractor = requirementExtractor;
            _context = context;
            _random = new Random();
        }

        public int RandomValue(int min, int max)
        {
            return _random.Next(min, max);
        }

        public void ValidateFilters(GameFilterDto filter)
        {
            ValidateCategoryFilter(filter);

            if (filter.Platform is not null)
            {
                var allowed = new[] { "pc", "browser", "all" };
                if (!allowed.Contains(filter.Platform.ToLower()))
                    throw new ArgumentException("RCR-105 - Invalid platform. Use: pc, browser or all.");
            }

            if (filter.RamMemory is not null && filter.RamMemory <= 0)
                throw new ArgumentException("RCR-105 - Invalid RAM.");
        }

        public void ValidateCategoryFilter(GameFilterDto filter)
        {
            if (string.IsNullOrWhiteSpace(filter.Category))
                throw new ArgumentException("RCR-105 - Category is required.");
            else
            {
                var categories = new[] { "mmorpg", "shooter", "moba", "anime", "battle royale", "strategy", "fantasy"
                                       , "sci-fi", "card games", "racing", "fighting", "social", "sports" };

                if (filter.Category.Contains("."))
                {
                    foreach (var c in filter.Category.Split("."))
                    {
                        if (!categories.Contains(c.ToLower())) throw new ArgumentException(@"RCR-105 - Invalid category. Use: " + string.Join(", ", categories) + ".");
                    }
                }
                else
                {
                    if (!categories.Contains(filter.Category.ToLower())) throw new ArgumentException(@"RCR-105 - Invalid category. Use: " + string.Join(", ", categories) + ".");
                }
            }            
        }

        public string BuildApiUrl(string category, string? platform)
        {
            string query = category.Contains(".")
                ? $"tag={category}" 
                : $"category={category}";

            if (!string.IsNullOrEmpty(platform)) 
                query += $"&platform={platform}";

            return $"https://www.freetogame.com/api/games?{query}";
        }

        public async Task<List<GameDto>> GamesFromApiAsync(string apiUrl)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new NotFoundException("RCR-102 - Game not found. Check parameters.");

                throw new HttpRequestException($"Freetogame API error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
                

            var json = await response.Content.ReadAsStringAsync();
            List<GameDto> games = JsonSerializer.Deserialize<List<GameDto>>(json);

            return games ?? new List<GameDto>();
        }

        public async Task<GameDto> CompatibleMemoryGameAsync(List<GameDto> games, int? maxRam, int maxAttempts = 500)
        {
            for (int i = 0; i < maxAttempts; i++)
            {
                var randomGame = games[RandomValue(0, games.Count - 1)];
                var minRam = await _requirementExtractor.ExtractMinimumRequirementsAsync(randomGame.FreetogameProfileUrl);

                if (minRam <= maxRam || maxRam == null)
                {
                    return randomGame;
                }

                // Add some time to avoid access block
                await Task.Delay(200);
            }

            throw new Exception();
        }

        public async Task SaveOrUpdateGameRecommendationAsync(GameDto recommendedGame)
        {
                var gameDb = await _context
                    .Games
                    .FirstOrDefaultAsync(c => c.Title == recommendedGame.Title);

                if (gameDb is null)
                {
                    GameRecommended game = new GameRecommended()
                    {
                        Title = recommendedGame.Title,
                        Category = recommendedGame.Genre
                    };
                    await _context.AddAsync(game);
                }
                else
                {
                    gameDb.Counter++;
                    _context.Update(gameDb);
                }

            await _context.SaveChangesAsync();
        }
    }
}
