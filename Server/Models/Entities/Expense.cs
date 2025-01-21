using System;

namespace Server.Models.Entities
{
    public class Expense
    {
        public Guid ExpenseId { get; set; }
        public Guid? TripId { get; set; }
        public Guid PaidUser { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public string Category { get; set; }

        public Trip Trip { get; set; }
        public Users PaidUserNavigation { get; set; }
    }
}