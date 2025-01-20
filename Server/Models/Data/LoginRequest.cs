using System.ComponentModel.DataAnnotations;

namespace Server.Models.Data
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}