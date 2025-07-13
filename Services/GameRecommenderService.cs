using GameRecommenderAPI.Data;
using GameRecommenderAPI.Dtos;
using GameRecommenderAPI.Models;
using GameRecommenderAPI.Services.Requirements;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace GameRecommenderAPI.Services
{
    public class GameRecommenderService
    {
        private readonly IRequirementExtractor _requirementExtractor;
        private readonly GameRecommenderDataContext _context;
        private readonly Random _random;
        public GameRecommenderService(GameRecommenderDataContext context, IRequirementExtractor requirementExtractor) 
        {
            _requirementExtractor = requirementExtractor;
            _context = context;
            _random = new Random();
        }

        public int RandomValue(int min, int max)
        {
            return _random.Next(min, max);
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
