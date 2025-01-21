using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Models.Entities;
using Server.Models.Data;

namespace Server.Services
{
    public class UpdateTripStatus
    {
        private readonly ApplicationDbContext _context; 

        public UpdateTripStatus(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserExpense>> FetchUserExpenseAsync(Guid tripId)
        {
            

             var userExpenses = await  _context.TripParticipants
            .Where(tp => tp.TripId == tripId)
            .GroupJoin(
                _context.Expenses.Where(e => e.TripId == tripId),
                tp => tp.UserId,
                e => e.PaidUser,
                (tp, expenses) => new
                {
                    UserId = tp.UserId,
                    TotalPaid = expenses.Sum(e => (decimal?)e.Amount) ?? 0 // Handle null values with 0
                }
            )
            .Select(r => new UserExpense
            {
                UserId = r.UserId,
                Amount = r.TotalPaid
            })
            .ToListAsync();

            return userExpenses;
        }

        // public async Task SettleExpensesAsync(Guid tripId, List<UserExpense> expenses)
         public async Task<List<Debt>> SettleExpensesAsync(Guid tripId, List<UserExpense> expenses)
        {
            int numPeople = expenses.Count;
            decimal totalExpense = expenses.Sum(e => e.Amount);
            decimal averageExpense = totalExpense / numPeople;

            // Calculate net balances for each user
            Dictionary<Guid, decimal> netBalances = new Dictionary<Guid, decimal>();
            foreach (var expense in expenses)
            {
                netBalances[expense.UserId] = expense.Amount - averageExpense;
            }

            // Separate creditors and debtors
            List<KeyValuePair<Guid, decimal>> creditors = netBalances
                .Where(kvp => kvp.Value > 0)
                .Select(kvp => new KeyValuePair<Guid, decimal>(kvp.Key, kvp.Value))
                .ToList();

            List<KeyValuePair<Guid, decimal>> debtors = netBalances
                .Where(kvp => kvp.Value < 0)
                .Select(kvp => new KeyValuePair<Guid, decimal>(kvp.Key, -kvp.Value))
                .ToList();

            // Settle balances
            int i = 0, j = 0;
            var debts = new List<Debt>(); // To batch insert later

            while (i < debtors.Count && j < creditors.Count)
            {
                Guid debtorId = debtors[i].Key;
                Guid creditorId = creditors[j].Key;

                decimal debtorBalance = debtors[i].Value;
                decimal creditorBalance = creditors[j].Value;

                // Calculate the payment amount
                decimal payment = Math.Min(debtorBalance, creditorBalance);

                // Create a new debt entry
                debts.Add(new Debt
                {
                    TripId = tripId,
                    DebtorId = debtorId,
                    CreditorId = creditorId,
                    Amount = (decimal)payment,
                    Status = false // Assuming false indicates the debt is unpaid
                });

                // Adjust balances
                debtors[i] = new KeyValuePair<Guid, decimal>(debtorId, debtorBalance - payment);
                creditors[j] = new KeyValuePair<Guid, decimal>(creditorId, creditorBalance - payment);

                // Move to the next debtor or creditor if their balance is settled
                if (debtors[i].Value == 0) i++;
                if (creditors[j].Value == 0) j++;
            }

            // Save debts to the database
            await _context.Debts.AddRangeAsync(debts);
            await _context.SaveChangesAsync();

             return debts;
        }
    }
}
