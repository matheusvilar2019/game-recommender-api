using HtmlAgilityPack;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace GameRecommenderAPI.Services.Requirements
{
    public class HtmlRequirementExtractor : IRequirementExtractor
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HtmlRequirementExtractor(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<int?> ExtractMinimumRequirementsAsync(string url)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var html = await httpClient.GetStringAsync(url);

                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var headerNode = doc.DocumentNode.SelectSingleNode("//span[contains(text(), 'Memory')]");
                if (headerNode == null)
                    return 0;

                // Memory paragraph <p>. Almost always after the h1 Memory
                var ramRow = headerNode.SelectSingleNode("following-sibling::p[1]");
                if (ramRow == null)
                    return 0;

                var ramText = ramRow.InnerHtml;
                if (ramText == null)
                    return 0;

                // regex: Extract the memory value
                var match = Regex.Match(ramText, @"(\d+)\s*GB", RegexOptions.IgnoreCase);
                if (match.Success && int.TryParse(match.Groups[1].Value, out var ramGb))
                {
                    return ramGb;
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
