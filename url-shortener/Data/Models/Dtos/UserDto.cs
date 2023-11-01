using url_shortener.Data.Models.Eums;

namespace url_shortener.Data.Models.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public RoleEnum Role { get; set; } = RoleEnum.User;
    }
}
