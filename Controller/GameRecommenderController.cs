using GameRecommenderAPI.Dtos;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Xml;

namespace GameRecommenderAPI.Controller
{
    [ApiController]
    public class GameRecommenderController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GameRecommenderController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("v1/recommender")]
        public async Task<IActionResult> Recommender([FromBody] ParametersDto parameters)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync("https://www.freetogame.com/api/games?platform=pc");

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, "Erro ao buscar dados da FreeToGame");

                var json = await response.Content.ReadAsStringAsync();
                var games = JsonSerializer.Deserialize<List<GameDto>>(json);

                return Ok(games);
            } 
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
            
        }
    }
}
