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
        public async Task<IActionResult> CreateTrip([FromBody] Trip trip)
        {
            if (string.IsNullOrEmpty(trip.TripName))
            {
                return BadRequest(new { message = "Trip name is required." });
            }

            trip.TripId = Guid.NewGuid();
            trip.TripDate = DateTime.UtcNow; // Set the trip date to the current date and time

            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Trip created successfully" });
        }

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