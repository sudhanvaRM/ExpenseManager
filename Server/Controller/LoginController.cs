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
               return BadRequest(new { message = "Username and password are required." });        
            }

            var user = await _context.Users
                .Where(u => u.Username == loginRequest.Username)
                .Select(u => new { u.Username, u.Password, u.UserId })
                .FirstOrDefaultAsync();


            if (user == null)
            {
                return Unauthorized(new {message ="Invalid username or password."});
            }


            if (user.Password != loginRequest.Password)
            {
                return Unauthorized(new {message ="Invalid username or password."});
            }

            // Console.WriteLine($"Login successful for username: {loginRequest.Username}");
            // Here you can generate and return a JWT token or any other authentication token
            return Ok(new {message = "Login successful", user_id = user.UserId});
        }
    }
}