using System;

namespace Server.Models.Data
{
    public class AddExpense
    {
        public Guid? TripId { get; set; }  // Nullable since TripId may or may not be provided
        public Guid PaidUser { get; set; }  // The user who paid the expense
        public decimal Amount { get; set; }  // The expense amount
        public string Comment { get; set; }  // A brief description of the expense
        public string Category { get; set; }  // Category of the expense (e.g., Food, Travel)
    }
}
