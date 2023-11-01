using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace url_shortener.Data.Entities
{
    public class Url
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string FullUrl { get; set; }
        [Required]
        public string ShortUrl { get; set; }
        public int Clicks { get; set; } = 0;
        [ForeignKey("UserId")]
        public int UserId { get; set; }
    }
}
