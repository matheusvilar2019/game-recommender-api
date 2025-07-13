using GameRecommenderAPI.Dtos;
using GameRecommenderAPI.Services.Requirements;
using HtmlAgilityPack;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace GameRecommenderAPI.Services
{
    public class GameRecommenderService
    {
        private readonly IRequirementExtractor _requirementExtractor;
        private readonly Random _random;
        public GameRecommenderService(IRequirementExtractor requirementExtractor) 
        {
            _requirementExtractor = requirementExtractor;
            _random = new Random();
        }

        public int RandomValue(int min, int max)
        {
            return _random.Next(min, max);
        }

        public async Task<GameDto> CompatibleMemoryGame(List<GameDto> games, int? maxRam, int maxAttempts = 500)
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
    }
}
