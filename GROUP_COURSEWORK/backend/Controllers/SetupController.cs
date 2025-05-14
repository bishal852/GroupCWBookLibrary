using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetupController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SetupController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("create-admin")]
        public async Task<IActionResult> CreateAdmin()
        {
            try
            {
                // Check if admin already exists
                var existingAdmin = await _context.Users.FirstOrDefaultAsync(u => u.Email == "admin@admin.com");
                if (existingAdmin != null)
                {
                    // Delete existing admin to recreate with fresh credentials
                    _context.Users.Remove(existingAdmin);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Existing admin user removed");
                }

                // Create new admin with fresh password hash
                string password = "admin123";
                using var hmac = new HMACSHA512();
                
                var admin = new User
                {
                    Name = "Admin User",
                    Email = "admin@admin.com",
                    PasswordSalt = hmac.Key,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Users.AddAsync(admin);
                await _context.SaveChangesAsync();

                // Verify the password hash works
                bool verificationTest = VerifyPasswordHash(password, admin.PasswordHash, admin.PasswordSalt);
                
                return Ok(new { 
                    message = "Admin user created successfully", 
                    email = admin.Email, 
                    password = password,
                    passwordVerificationTest = verificationTest
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error creating admin: {ex.Message}" });
            }
        }

        // Helper method to verify password hash (same as in UserService)
        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i])
                {
                    return false;
                }
            }
            
            return true;
        }

        [HttpGet("verify-admin")]
        public async Task<IActionResult> VerifyAdmin()
        {
            try
            {
                var admin = await _context.Users.FirstOrDefaultAsync(u => u.Email == "admin@admin.com");
                if (admin == null)
                {
                    return NotFound(new { message = "Admin user not found" });
                }

                // Test password verification
                bool verificationTest = VerifyPasswordHash("admin123", admin.PasswordHash, admin.PasswordSalt);

                return Ok(new {
                    adminExists = true,
                    email = admin.Email,
                    role = admin.Role,
                    isActive = admin.IsActive,
                    passwordVerificationTest = verificationTest,
                    passwordHashLength = admin.PasswordHash?.Length,
                    passwordSaltLength = admin.PasswordSalt?.Length
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error verifying admin: {ex.Message}" });
            }
        }
    }
}
