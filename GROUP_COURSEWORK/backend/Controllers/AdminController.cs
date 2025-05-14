using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Services;
using backend.DTOs;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("createstaff")]
        public async Task<IActionResult> CreateStaff([FromBody] CreateStaffDto createStaffDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if email already exists
            if (await _userService.EmailExistsAsync(createStaffDto.Email))
            {
                return BadRequest(new { message = "Email already in use" });
            }

            // Create user with 'Staff' role
            var user = new User
            {
                Name = createStaffDto.Name,
                Email = createStaffDto.Email,
                Role = "Staff"
            };

            // Hash the password
            var (passwordHash, passwordSalt) = _userService.CreatePasswordHash(createStaffDto.Password);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            // Save user to database
            await _userService.CreateUserAsync(user);

            return Ok(new { message = "Staff created successfully" });
        }
    }
}
