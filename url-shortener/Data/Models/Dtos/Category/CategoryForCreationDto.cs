using System.ComponentModel.DataAnnotations;

namespace url_shortener.Data.Models.Dtos.Category
{
    public class CategoryForCreationDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
