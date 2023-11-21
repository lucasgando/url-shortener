using url_shortener.Data.Entities;
using url_shortener.Data.Models.Dtos.Category;
using url_shortener.Data.Models.Dtos.Url;
using url_shortener.Data.Models.Dtos.User;

namespace url_shortener.Helpers
{
    public static class DtoHandler
    {
        public static UserDto GetUserDto(User user)
        {
            return new UserDto()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                Role = user.Role
            };
        }
        public static UrlDto GetUrlDto(Url url)
        {
            return new UrlDto()
            {
                Id = url.Id,
                FullUrl = url.FullUrl,
                ShortUrl = url.ShortUrl,
                Clicks = url.Clicks,
                Categories = url.Categories.Select(x => GetCategoryDto(x)).ToList(),
                UserId = url.UserId
            };
        }
        public static CategoryDto GetCategoryDto(Category category)
        {
            return new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}
