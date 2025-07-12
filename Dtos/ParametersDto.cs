using System.ComponentModel.DataAnnotations;

namespace GameRecommenderAPI.Dtos
{
    public class ParametersDto
    {
        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }
        public string Platform { get; set; }
        public int RamMemory { get; set; }
    }
}
