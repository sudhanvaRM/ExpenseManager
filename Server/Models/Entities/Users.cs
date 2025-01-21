using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Models.Entities
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

        public ICollection<Trip_Participants> TripParticipants { get; set; }

        public ICollection<Expense> Expenses { get; set; }

        public virtual ICollection<Debt> DebtsAsDebtor { get; set; }
        public virtual ICollection<Debt> DebtsAsCreditor { get; set; }
    }
}