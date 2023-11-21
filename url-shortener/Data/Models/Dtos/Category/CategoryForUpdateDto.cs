using System.ComponentModel.DataAnnotations;

namespace url_shortener.Data.Models.Dtos.Category
{
    public class CategoryForUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
