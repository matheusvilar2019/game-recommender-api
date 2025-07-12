using GameRecommenderAPI.Dtos;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using static System.Net.WebRequestMethods;

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
                string apiUrl = $"https://www.freetogame.com/api/games?category={parameters.Category}&platform={parameters.Platform}";
                var response = await httpClient.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);

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
