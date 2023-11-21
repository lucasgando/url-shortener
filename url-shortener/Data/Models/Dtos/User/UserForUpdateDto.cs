using System.ComponentModel.DataAnnotations;

namespace url_shortener.Data.Models.Dtos.User
{
    public class UserForUpdateDto
    {
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
