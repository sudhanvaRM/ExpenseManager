using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Users
    {
        [Key]
        public Guid UserId { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(255)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }
    }
}