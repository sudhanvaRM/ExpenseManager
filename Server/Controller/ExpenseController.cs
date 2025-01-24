using Microsoft.AspNetCore.Mvc;
using Server.Models.Entities;
using Server.Services;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Server.Models.Data;

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

       [HttpGet("NotInTrip/{tripId}")]
        public ActionResult<IEnumerable<object>> GetUsersNotInTrip(Guid tripId)
        {
            try
            {
                var usersNotInTrip = _context.Users
                    .Where(u => !_context.TripParticipants
                        .Any(tp => tp.TripId == tripId && tp.UserId == u.UserId))
                    .Select(u => new
                    {
                        u.UserId,
                        u.Username
                    })
                    .ToList();

                return Ok(usersNotInTrip);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the data.", error = ex.Message });
            }
        }

        [HttpPost("add-person-to-trip")]
        public async Task<IActionResult> AddPersonToTrip(AddTrip addTrip)
        {
            // Check if the trip exists
            var trip = await _context.Trips.FindAsync(addTrip.TripId);

            if (trip == null)
            {
                return NotFound(new { message = "Trip not found." });
            }

            // Add the user to the trip
            Trip_Participants tripParticipant = new Trip_Participants
            {
                TripId =   addTrip.TripId,
                UserId = addTrip.UserId
            };

            _context.TripParticipants.Add(tripParticipant);
            await _context.SaveChangesAsync();
            List<UserExpense> userExpenses = await _tripStatus.FetchUserExpenseAsync(tripParticipant.TripId);
            List<Debt> debts = await _tripStatus.SettleExpensesAsync(tripParticipant.TripId, userExpenses);

            return Ok(new { message = "User added to trip successfully." });
        }

        [HttpPost("add-expense")]   
        // Add the expense to the database
        public async Task<IActionResult> AddExpense([FromBody] AddExpense expenseDto)
        {
            if (expenseDto == null)
            {
                return BadRequest("Expense data is required.");
            }

            // Start a database transaction
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Generate ExpenseId on the server
                var expense = new Expense
                {
                    ExpenseId = Guid.NewGuid(),
                    TripId = expenseDto.TripId,
                    PaidUser = expenseDto.PaidUser,
                    Amount = expenseDto.Amount,
                    Comment = expenseDto.Comment,
                    Category = expenseDto.Category
                };

                if (expense.TripId.HasValue)
                {
                    // Fetch the trip participant record
                    var tripParticipant = await _context.TripParticipants
                        .FirstOrDefaultAsync(tp => tp.TripId == expense.TripId.Value && tp.UserId == expense.PaidUser);

                    // Check if the user is not part of the trip
                    if (tripParticipant == null)
                    {
                        return NotFound($"Invalid Trip Details or User is not part of this trip.");
                    }
                }

                // Validate PaidUser exists
                var userExists = await _context.Users.FindAsync(expense.PaidUser);
                if (userExists == null)
                {
                    return NotFound($"Invalid User Details");
                }

                // Add the expense
                await _context.Expenses.AddAsync(expense);
                await _context.SaveChangesAsync();

                // Additional processing for TripId
                if (expense.TripId.HasValue)
                {
                    List<UserExpense> userExpenses = await _tripStatus.FetchUserExpenseAsync(expense.TripId.Value);

                    var debtExisting = await _context.Debts
                              .Where(d => d.TripId == expense.TripId)
                              .ToListAsync();

                     // Remove the debts from the context
                    _context.Debts.RemoveRange(debtExisting);

                       // Save changes to the database
                     await _context.SaveChangesAsync();   

                    //Add New Debt Status     
                    List<Debt> debts = await _tripStatus.SettleExpensesAsync(expense.TripId.Value, userExpenses);

                    // If needed, save any changes resulting from debt settlement
                    await _context.SaveChangesAsync();
                }

                // Commit the transaction
                await transaction.CommitAsync();

                return Ok(new { Message = "Expense added successfully", ExpenseId = expense.ExpenseId });
            }
            catch (Exception ex)
            {
                // Roll back the transaction on any failure
                await transaction.RollbackAsync();

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("user-expenses/{userId}")]  
        public async Task<IActionResult> GetUserExpenses(Guid userId)
        {
            var expenses = await _context.Expenses
                .Where(e => e.PaidUser == userId)
                .Select(e => new 
                {
                    ExpenseId = e.ExpenseId,
                    Amount = e.Amount,
                    Comment = e.Comment,
                    Category = e.Category,
                    TripName = e.TripId.HasValue ? e.Trip.TripName : "Unassigned",
                    TripId = e.TripId
                })
                .ToListAsync();

            if (expenses == null || expenses.Count == 0)
            {
                return NotFound(new { message = "No expenses found for the user." });
            }

            return Ok(expenses);
        }

        [HttpPost("assign-trip")]
        public async Task<IActionResult> AssignTrip([FromBody] AssignTripRequest request)
        {
            var expense = await _context.Expenses.FindAsync(request.ExpenseId);
            if (expense == null)
            {
                return NotFound("Expense not found.");
            }

            // Update the TripId of the expense
            expense.TripId = request.TripId;
            await _context.SaveChangesAsync();

            List<UserExpense> userExpenses = await _tripStatus.FetchUserExpenseAsync(expense.TripId.Value);

            var debtExisting = await _context.Debts
                              .Where(d => d.TripId == expense.TripId)
                              .ToListAsync();

             _context.Debts.RemoveRange(debtExisting);

                       // Save changes to the database

                     await _context.SaveChangesAsync();   
                     
             List<Debt> debts = await _tripStatus.SettleExpensesAsync(expense.TripId.Value, userExpenses);

                    // If needed, save any changes resulting from debt settlement
                    await _context.SaveChangesAsync();

            return Ok(new { message = "Expense assigned to trip successfully." });
        }

        // [HttpPost("pay-due")]
        // {

        // }

        // [HttpGet("get-dues")]
        // {

        // }

    }
}