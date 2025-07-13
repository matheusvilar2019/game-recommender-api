using GameRecommenderAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using GameRecommenderAPI.Services;
using GameRecommenderAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace GameRecommenderAPI.Controller
{
    [ApiController]
    public class GameRecommenderController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly GameRecommenderService _gameRecommenderService;
        private readonly ILogger<GameRecommenderController> _logger;

        public GameRecommenderController(IHttpClientFactory httpClientFactory, 
                                         GameRecommenderService gameRecommenderService,
                                         ILogger<GameRecommenderController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _gameRecommenderService = gameRecommenderService;
            _logger = logger;

        }

        [HttpGet("v1/recommender")]
        public async Task<IActionResult> GetRecommendedGameAsync([FromQuery] string category,
                                                                 [FromQuery] string? platform,
                                                                 [FromQuery] int? ramMemory)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                string query = string.Empty;

                if (!category.Contains(".")) 
                    query = $"category={category}";
                else
                    query = $"tag={category}";
                if (!string.IsNullOrEmpty(platform))
                    query += $"&platform={platform}";

                string apiUrl = $"https://www.freetogame.com/api/games?" + query;

                var response = await httpClient.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, "RCR-101 - " + response.ReasonPhrase);

                var json = await response.Content.ReadAsStringAsync();
                List<GameDto> games = JsonSerializer.Deserialize<List<GameDto>>(json);

                if (games == null || !games.Any())
                    return NotFound("RCR-102 - Game not found.");

                GameDto recommendedGame = await _gameRecommenderService.CompatibleMemoryGameAsync(games, ramMemory);

                await _gameRecommenderService.SaveOrUpdateGameRecommendationAsync(recommendedGame);

                GameResponseDto responseDto = new GameResponseDto()
                {
                    Title = recommendedGame.Title,
                    FreetogameProfileUrl = recommendedGame.FreetogameProfileUrl
                };

                return Ok(responseDto);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "RCR-103 - Internal server error");
                return StatusCode(500, "RCR-103 - Internal server error");
            }            
        }

        [HttpGet("v1/all-games-recommended")]
        public async Task<IActionResult> GetAllGamesRecommendedAsync([FromServices] GameRecommenderDataContext context)
        {
            try
            {
                var games = await context
                    .Games
                    .ToListAsync();

                if (games == null) return NoContent();

                return Ok(games);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AGR-101 - Internal server error");
                return StatusCode(500, "AGR-101 - Internal server error");
            }
        }
    }
}
