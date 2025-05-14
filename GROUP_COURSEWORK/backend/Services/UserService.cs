using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAndRoleAsync(string email, string role)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Role == role);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            Console.WriteLine($"Looking for user with email: {email}");
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
            
            if (user != null)
            {
                Console.WriteLine($"User found: {user.Email}, Role: {user.Role}, IsActive: {user.IsActive}");
                Console.WriteLine($"PasswordHash length: {user.PasswordHash?.Length}, PasswordSalt length: {user.PasswordSalt?.Length}");
            }
            else
            {
                Console.WriteLine("User not found");
            }
            
            return user;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public (byte[] passwordHash, byte[] passwordSalt) CreatePasswordHash(string password)
        {
            using var hmac = new HMACSHA512();
            var passwordSalt = hmac.Key;
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            
            return (passwordHash, passwordSalt);
        }

        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (storedHash == null) throw new ArgumentNullException(nameof(storedHash));
            if (storedSalt == null) throw new ArgumentNullException(nameof(storedSalt));
            
            Console.WriteLine($"Verifying password. Hash length: {storedHash.Length}, Salt length: {storedSalt.Length}");
            
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            
            Console.WriteLine($"Computed hash length: {computedHash.Length}");
            
            // Debug: Print first few bytes of both hashes
            Console.WriteLine("Stored hash (first 5 bytes): " + BitConverter.ToString(storedHash, 0, 5));
            Console.WriteLine("Computed hash (first 5 bytes): " + BitConverter.ToString(computedHash, 0, 5));
            
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i])
                {
                    Console.WriteLine($"Hash mismatch at position {i}: {computedHash[i]} != {storedHash[i]}");
                    return false;
                }
            }
            
            Console.WriteLine("Password hash verification succeeded");
            return true;
        }
    }
}
