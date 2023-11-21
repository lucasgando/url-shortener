using url_shortener.Data.Models.Dtos.Category;

namespace url_shortener.Data.Models.Dtos.Url
{
    public class UrlDto
    {
        public int Id { get; set; }
        public string FullUrl { get; set; }
        public string ShortUrl { get; set; }
        public int Clicks { get; set; } = 0;
        public int UserId { get; set; }

        public List<CategoryDto> Categories { get; set; }
    }
}
