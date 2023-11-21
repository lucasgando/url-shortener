using System.ComponentModel.DataAnnotations;

namespace url_shortener.Data.Models.Dtos.User
{
    public class UserForDeletionDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
