using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models.Data;
using System.Linq;
using System.Threading.Tasks;
using Server.Models.Entities;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignupController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SignupController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost()]
        public async Task<IActionResult> Signup([FromBody] LoginRequest signupRequest)
        {
            if (string.IsNullOrEmpty(signupRequest.Username) || string.IsNullOrEmpty(signupRequest.Password))
            {
                return BadRequest(new { message = "Username and password are required." });
            }

            var existingUser = await _context.Users
                .Where(u => u.Username == signupRequest.Username)
                .FirstOrDefaultAsync();

            if (existingUser != null)
            {
                return Conflict(new { message = "Username exists." });
            }

            var newUser = new Users
            {
                Username = signupRequest.Username,
                Password = signupRequest.Password
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Signup successful", user_id = newUser.UserId });
        }
    }

}