using System.ComponentModel.DataAnnotations;

namespace Agape.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        // Agrega más propiedades según necesites
    }
}
