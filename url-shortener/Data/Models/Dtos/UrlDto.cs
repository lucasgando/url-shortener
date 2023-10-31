using System.ComponentModel.DataAnnotations;

namespace url_shortener.Data.Models.Dtos
{
    public class UrlDto
    {
        public string FullUrl { get; set; }
        public int UserId { get; set; }
    }
}
