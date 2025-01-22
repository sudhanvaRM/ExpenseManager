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