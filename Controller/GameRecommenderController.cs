using GameRecommenderAPI.Dtos;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using static System.Net.WebRequestMethods;
using GameRecommenderAPI.Services;
using GameRecommenderAPI.Models;
using GameRecommenderAPI.Data;

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
        public async Task<IActionResult> Recommender([FromQuery] string category,
                                                     [FromQuery] string? platform,
                                                     [FromQuery] int? ramMemory,
                                                     [FromServices] GameRecommenderDataContext context)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();

                string query = $"category={category}";                 
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

                GameDto recommendedGame = await _gameRecommenderService.CompatibleMemoryGame(games, ramMemory);

                // Saving recommended game 
                GameRecommended game = new GameRecommended()
                {
                    Name = recommendedGame.Title,
                    Category = recommendedGame.Genre,
                };

                await context.AddAsync(game);
                await context.SaveChangesAsync();

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

        
    }
}
