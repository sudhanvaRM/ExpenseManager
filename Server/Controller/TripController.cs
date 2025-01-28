using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Models.Data;
using Server.Models.Entities;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TripController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("add-trip")]
        public async Task<IActionResult> CreateTrip([FromBody] TripData tripData)
        {
            if (string.IsNullOrEmpty(tripData.TripName))
            {
                return BadRequest(new { message = "Trip name is required." });
            }

            var trip = new Trip
            {
                TripId = Guid.NewGuid(),
                TripName = tripData.TripName,
                TripDate = DateTime.UtcNow // Set the trip date to the current date and time
            };

            _context.Trips.Add(trip);

            var tripParticipant = new Trip_Participants
            {
                TripId = trip.TripId,
                UserId = tripData.UserId
            };

            _context.TripParticipants.Add(tripParticipant);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Trip created successfully", tripId = trip.TripId });
        }

         [HttpGet("user-trips/{userId}")]
         public async Task<IActionResult> GetUserTrips(Guid userId)
         {
            var trips = await _context.TripParticipants
                .Where(tp => tp.UserId == userId)
                .Select(tp => new { tp.Trip.TripId, tp.Trip.TripName })
                .ToListAsync();

            if (trips == null || trips.Count == 0)
            {
                return NotFound(new { message = "No trips found for the user." });
            }

            return Ok(trips);
         }

         [HttpGet("trip-details/{userId}")]
        public async Task<IActionResult> GetTripDetails(Guid userId)
        {
            var tripDetails = await _context.TripParticipants
                .Where(tp => tp.UserId == userId)
                .Select(tp => new { tp.Trip.TripId, tp.Trip.TripName })
                .ToListAsync();

            if (tripDetails == null || tripDetails.Count == 0)
            {
                return Ok(new { message = "No trips found for the user." });
            }

            return Ok(tripDetails);
        }

        [HttpPost("fetch-trip-debts")]
        public async Task<IActionResult> GetDebtsForTrip(AddTrip trip)
        {
            // Validate the user exists
            var userExists = await _context.Set<Users>().AnyAsync(u => u.UserId == trip.UserId);
            if (!userExists)
            {
                return NotFound(new { Message = "User not found." });
            }

            // Validate the trip exists
            var tripExists = await _context.Set<Trip>().AnyAsync(t => t.TripId == trip.TripId);
            if (!tripExists)
            {
                return NotFound(new { Message = "Trip not found." });
            }

            // Debts where the user is the debtor (owes money) for the specified trip
            var owesToOthers = await _context.Set<Debt>()
                .Where(d => d.DebtorId == trip.UserId && d.TripId == trip.TripId && d.Status == false)
                .Select(d => new
                {   
                    TripId = d.TripId,
                    CreditorId = d.CreditorId,
                    CreditorUsername = d.Creditor.Username,
                    Amount = d.Amount
                })
                .ToListAsync();

            // Debts where the user is the creditor (money owed to the user) for the specified trip
            var owedByOthers = await _context.Set<Debt>()
                .Where(d => d.CreditorId == trip.UserId && d.TripId == trip.TripId && d.Status == false)
                .Select(d => new
                {
                    TripId = d.TripId,
                    DebtorId = d.DebtorId,
                    DebtorUsername = d.Debtor.Username,
                    Amount = d.Amount
                })
                .ToListAsync();

            return Ok(new
            {
                OwesToOthers = owesToOthers,
                OwedByOthers = owedByOthers
            });
        }



[HttpPost("settle-debt")]
public async Task<IActionResult> SettleDebt(SettleDebtRequest debt)
{

    using (var transaction = await _context.Database.BeginTransactionAsync())
    {
        try
        {
            // Find the debt record
            var debtDetails = await _context.Debts
                .Where(d => d.TripId == debt.TripId && d.DebtorId == debt.DebtorId && d.CreditorId == debt.CreditorId)
                .FirstOrDefaultAsync();
                


            if (debtDetails == null)
            {
                return Ok(new { message = "Debt not found" });
            }

            // Update debt status to true (settled)
            debtDetails.Status = true;
            _context.Entry(debtDetails).State = EntityState.Modified;

            // Create a new expense for the debtor
            var debtorExpense = new Expense
            {
                ExpenseId = Guid.NewGuid(),
                TripId = debtDetails.TripId,
                PaidUser = debtDetails.DebtorId,
                Amount = debtDetails.Amount,
                Comment = "Settled payment",
                Category = "Expense Settlement"
            };

            await _context.Expenses.AddAsync(debtorExpense);

            // Subtract the settled amount from one of the creditor's expenses for the same trip
            var creditorExpense = await _context.Expenses
                .Where(e => e.PaidUser == debtDetails.CreditorId && e.TripId == debtDetails.TripId && e.Amount > 0)
                .OrderByDescending(e => e.Amount) // Choose the largest expense
                .FirstOrDefaultAsync();

            if (creditorExpense != null)
            {
                creditorExpense.Amount -= debtDetails.Amount;
                _context.Entry(creditorExpense).State = EntityState.Modified;
            }

            // Save changes for both the debt status update and new/updated expenses
            await _context.SaveChangesAsync();

            // Commit the transaction
            await transaction.CommitAsync();

            return Ok(new { message = "Debt settled successfully" });
        }
        catch (Exception ex)
        {
            // Rollback the transaction if an error occurs
            await transaction.RollbackAsync();
            return StatusCode(500, new { message = "An error occurred while settling the debt", error = ex.Message });
        }
    }
}



        
        
        

        
        
        
        

            // [HttpGet("user-expenses/{userId}")]
            // public async Task<IActionResult> GetUserExpenses(Guid userId)
            // {
            //     var expenses = await _context.Expenses
            //         .Where(e => e.PaidUser == userId)
            //         .Select(e => new 
            //         {
            //             ExpenseId = e.ExpenseId,
            //             Amount = e.Amount,
            //             Comment = e.Comment,
            //             Category = e.Category,
            //             TripName = e.TripId.HasValue ? e.Trip.TripName : "Unassigned"
            //         })
            //         .ToListAsync();

            //     if (expenses == null || expenses.Count == 0)
            //     {
            //         return NotFound(new { message = "No expenses found for the user." });
            //     }

            //     return Ok(expenses);
            // }


        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<Trip>>> GetAllTrips()
        // {
        //     return await _context.Trips.ToListAsync();
        // }

        // [HttpGet("{id}")]
        // public async Task<ActionResult<Trip>> GetTripById(Guid id)
        // {
        //     var trip = await _context.Trips.FindAsync(id);

        //     if (trip == null)
        //     {
        //         return NotFound(new { message = "Trip not found" });
        //     }

        //     return trip;
        // }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> UpdateTrip(Guid id, [FromBody] Trip trip)
        // {
        //     if (id != trip.TripId)
        //     {
        //         return BadRequest(new { message = "Trip ID mismatch" });
        //     }

        //     _context.Entry(trip).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!TripExists(id))
        //         {
        //             return NotFound(new { message = "Trip not found" });
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteTrip(Guid id)
        // {
        //     var trip = await _context.Trips.FindAsync(id);

        //     if (trip == null)
        //     {
        //         return NotFound(new { message = "Trip not found" });
        //     }

        //     _context.Trips.Remove(trip);
        //     await _context.SaveChangesAsync();

        //     return Ok(new { message = "Trip deleted successfully" });
        // }

        // private bool TripExists(Guid id)
        // {
        //     return _context.Trips.Any(e => e.TripId == id);
        // }
    }
}