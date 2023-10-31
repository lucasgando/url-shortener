using System.ComponentModel.DataAnnotations;

namespace url_shortener.Data.Models.Dtos
{
    public class UrlForCreationDto
    {
        [Required]
        [Url]
        public string Url { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
