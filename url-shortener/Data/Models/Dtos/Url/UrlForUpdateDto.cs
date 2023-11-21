using System.ComponentModel.DataAnnotations;

namespace url_shortener.Data.Models.Dtos.Url
{
    public class UrlForUpdateDto
    {
        [Required]
        public int Id { get; set; }
        public List<int> NewCategories { get; set; }
        public List<int> DeleteCategories { get; set; }
    }
}
