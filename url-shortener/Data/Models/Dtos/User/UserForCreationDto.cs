using System.ComponentModel.DataAnnotations;

namespace url_shortener.Data.Models.Dtos.User
{
    public class UserForCreationDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
