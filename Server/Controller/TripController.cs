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
                return NotFound(new { message = "No trips found for the user." });
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
                .Where(d => d.DebtorId == trip.UserId && d.TripId == trip.TripId)
                .Select(d => new
                {
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

        [HttpPost("SettleDebt")]
public async Task<IActionResult> SettleTripDebt(Guid DebtId)
{

    
    var debt = await _context.Debts.FindAsync(DebtId);

    if (debt == null)
    {
        return NotFound(new { message = "Debt not found" });
    }

    

    // Update the status of the debt to settled
    debt.Status = true;
    _context.Entry(debt).State = EntityState.Modified;

    // Fetch the debtor and creditor
    var debtor = await _context.Users.FindAsync(debt.DebtorId);
    var creditor = await _context.Users.FindAsync(debt.CreditorId);

    if (debtor == null || creditor == null)
    {
        return NotFound(new { message = "Debtor or Creditor not found" });
    }

    // Create a new expense for the debtor with the settled amount
    var debtorExpense = new Expense
    {
        PaidUser = debt.DebtorId,
        TripId = debt.TripId,
        Amount = debt.Amount,
        Comment = $"Settled debt with {debtor.Username}",
        Category = "Settlement",
    };

    // Subtract the settled amount from the creditor's existing expense
    var creditorExpense = await _context.Expenses
                                        .Where(e => e.PaidUser== debt.CreditorId && e.TripId == debt.TripId)
                                        .FirstOrDefaultAsync();

    if (creditorExpense != null)
    {
        creditorExpense.Amount -= debt.Amount;
        _context.Entry(creditorExpense).State = EntityState.Modified;
    }
    else
    {
        return NotFound(new { message = "Creditor's expense not found" });
    }

    // Add the new debtor expense to the database
    _context.Expenses.Add(debtorExpense);

    try
    {
        // Save changes for both the debt status update and new/updated expenses
        await _context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = "An error occurred while updating the debt", details = ex.Message });
    }

    return Ok(new { message = "Debt settled successfully, new expenses recorded." });
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