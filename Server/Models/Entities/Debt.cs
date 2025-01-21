using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models.Entities
{
    public class Debt
    {
        [Key, Column(Order = 0)]
        public Guid TripId { get; set; }

        [Key, Column(Order = 1)]
        public Guid DebtorId { get; set; }

        [Key, Column(Order = 2)]
        public Guid CreditorId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }

        [Required]
        public bool Status { get; set; }

        [ForeignKey("TripId")]
        public virtual Trip Trip { get; set; }

        [ForeignKey("DebtorId")]
        public virtual Users Debtor { get; set; }

        [ForeignKey("CreditorId")]
        public virtual Users Creditor { get; set; }
    }
}