namespace GameRecommenderAPI.Services.Requirements
{
    public interface IRequirementExtractor
    {
        Task<int?> ExtractMinimumRequirementsAsync(string url);
    }
}
