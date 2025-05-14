using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using backend.Models;
using backend.Services;
using backend.DTOs;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;

        public AuthController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Console.WriteLine($"[Register] Attempt – Email: {dto.Email}");

            // Check if email already exists
            if (await _userService.EmailExistsAsync(dto.Email))
            {
                Console.WriteLine($"[Register] ❌ Email {dto.Email} already exists");
                return BadRequest(new { message = "Email already in use" });
            }

            // Create user with 'Member' role
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Role = "Member",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // Hash the password
            var (passwordHash, passwordSalt) = _userService.CreatePasswordHash(dto.Password);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            // Save user to database
            await _userService.CreateUserAsync(user);
            Console.WriteLine($"[Register] ✅ User {dto.Email} registered successfully");

            return Ok(new { message = "Registration successful" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Console.WriteLine($"[Login] Attempt – Email: {dto.Email}, Role: {dto.Role}");

            // 1) Email lookup
            var user = await _userService.GetUserByEmailAsync(dto.Email);
            if (user == null)
            {
                Console.WriteLine($"[Login] ❌ No user with email {dto.Email}");
                return Unauthorized(new { message = "Email not found." });
            }

            // 2) Role check
            if (!user.Role.Equals(dto.Role, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"[Login] ❌ Role mismatch: user.Role={user.Role}, requested={dto.Role}");
                return Unauthorized(new { message = $"User is not a '{dto.Role}'." });
            }

            // 3) Active flag
            if (!user.IsActive)
            {
                Console.WriteLine($"[Login] ❌ User {user.Email} is inactive");
                return Unauthorized(new { message = "Account deactivated." });
            }

            // 4) Password verification
            bool valid = _userService.VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt);
            Console.WriteLine($"[Login] 🔑 Password match for {user.Email}: {valid}");
            if (!valid)
                return Unauthorized(new { message = "Invalid password." });

            // 5) Success: update last login and issue JWT
            user.LastLoginAt = DateTime.UtcNow;
            await _userService.UpdateUserAsync(user);

            var token = GenerateJwtToken(user);
            Console.WriteLine($"[Login] ✅ JWT issued to {user.Email}");

            return Ok(new { token, user = new { user.Id, user.Name, user.Email, user.Role } });
        }

        // Simple test login endpoint that bypasses database
        [HttpPost("test-login")]
        public IActionResult TestLogin([FromBody] LoginDto dto)
        {
            Console.WriteLine($"[TestLogin] Attempt – Email: {dto.Email}, Role: {dto.Role}");

            // Hardcoded admin credentials for testing
            if (dto.Email == "admin@admin.com" && dto.Password == "admin123" && dto.Role == "Admin")
            {
                var user = new User
                {
                    Id = 999,
                    Name = "Test Admin",
                    Email = dto.Email,
                    Role = dto.Role
                };

                var token = GenerateJwtToken(user);
                Console.WriteLine($"[TestLogin] ✅ Test JWT issued to {dto.Email}");

                return Ok(new { token, user = new { user.Id, user.Name, user.Email, user.Role } });
            }

            Console.WriteLine($"[TestLogin] ❌ Invalid test credentials");
            return Unauthorized(new { message = "Invalid test credentials" });
        }

        private string GenerateJwtToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
