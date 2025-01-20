using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models.Data;
using Server.Models.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost()]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Username and password are required.");
            }

            Console.WriteLine($"Login request for username: {loginRequest.Username}");

            var user = await _context.Users
                .Where(u => u.Username == loginRequest.Username)
                // .Select(u => new { u.Username, u.Password })
                .FirstOrDefaultAsync();

            Console.WriteLine($"Fetched user: {user.Username}, Password: {user.Password}");

            if (user == null)
            {
                Console.WriteLine($"User not found: {loginRequest.Username}");
                return Unauthorized("Invalid username or password.");
            }


            if (user.Password != loginRequest.Password)
            {
                Console.WriteLine($"Invalid password for username: {loginRequest.Username}");
                return Unauthorized("Invalid username or password.");
            }

            Console.WriteLine($"Login successful for username: {loginRequest.Username}");
            // Here you can generate and return a JWT token or any other authentication token
            return Ok("Login successful");
        }
    }
}