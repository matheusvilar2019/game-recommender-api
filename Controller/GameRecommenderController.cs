using GameRecommenderAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using GameRecommenderAPI.Services;
using GameRecommenderAPI.Data;
using Microsoft.EntityFrameworkCore;
using GameRecommenderAPI.Exceptions;

namespace GameRecommenderAPI.Controller
{
    [ApiController]
    public class GameRecommenderController : ControllerBase
    {
        private readonly GameRecommenderService _gameRecommenderService;
        private readonly ILogger<GameRecommenderController> _logger;

        public GameRecommenderController(GameRecommenderService gameRecommenderService,
                                         ILogger<GameRecommenderController> logger)
        {
            _gameRecommenderService = gameRecommenderService;
            _logger = logger;
        }

        [HttpGet("v1/recommender")]
        public async Task<IActionResult> GetRecommendedGameAsync([FromQuery] GameFilterDto filter)
        {
            try
            {
                _gameRecommenderService.ValidateFilters(filter);

                string apiUrl = _gameRecommenderService.BuildApiUrl(filter.Category, filter.Platform);
                List<GameDto> games = await _gameRecommenderService.GamesFromApiAsync(apiUrl);

                if (games == null || !games.Any())
                    return NotFound("RCR-101 - Game not found. Check parameters.");

                GameDto recommendedGame = await _gameRecommenderService.CompatibleMemoryGameAsync(games, filter.RamMemory);
                await _gameRecommenderService.SaveOrUpdateGameRecommendationAsync(recommendedGame);

                return Ok(new GameResponseDto()
                {
                    Title = recommendedGame.Title,
                    FreetogameProfileUrl = recommendedGame.FreetogameProfileUrl
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "RCR-102 - API returned 404");
                return NotFound(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "RCR-103 - API call failed");
                return StatusCode(502, "RCR-103 - " + ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RCR-104 - Internal server error");
                return StatusCode(500, "RCR-104 - Internal server error");
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
