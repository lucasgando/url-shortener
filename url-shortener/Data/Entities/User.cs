using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using url_shortener.Data.Models.Eums;

namespace url_shortener.Data.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public RoleEnum Role { get; set; } = RoleEnum.User;
        public List<Category> Categories { get; set; }
        public List<Url> Urls { get; set; }
    }
}
