using Microsoft.AspNetCore.Mvc;
using Server.Models.Entities;
using Server.Services;
using System;
using System.Linq;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UpdateTripStatus _tripStatus;

        public ExpenseController(ApplicationDbContext context,UpdateTripStatus tripStatus)
        {
            _context = context;
            _tripStatus = tripStatus;

        }

        // Existing methods...

        [HttpGet("update-trip-status")]
        public async Task<IActionResult> GetTripStatus(Guid tripId)
        {
            List<UserExpense> userExpenses = await _tripStatus.FetchUserExpenseAsync(tripId);
            //  List<Debt> debts = await _tripStatus.SettleExpensesAsync(tripId, userExpenses);
            
            // You can now return the result or perform any other operations
            // return Ok(userExpenses); // Example: returning the list of user expenses as JSON
            return Ok(userExpenses);
        }

    }
}