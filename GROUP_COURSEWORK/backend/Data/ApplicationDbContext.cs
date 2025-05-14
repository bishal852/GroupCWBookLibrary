using Microsoft.EntityFrameworkCore;
using backend.Models;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public object CartItems { get; internal set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.User)
                .WithMany(u => u.Wishlists)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.Book)
                .WithMany()
                .HasForeignKey(w => w.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Remove the explicit relationship configuration for ProcessedByStaff
            // since we're using [NotMapped]

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Book)
                .WithMany()
                .HasForeignKey(oi => oi.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Book)
                .WithMany()
                .HasForeignKey(r => r.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Book)
                .WithMany()
                .HasForeignKey(c => c.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed default admin user
            // Create password hash and salt for admin
            CreatePasswordHash("Admin123!", out byte[] passwordHash, out byte[] passwordSalt);
            
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Admin",
                    Email = "admin@admin.com",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            );

            // Get current UTC time for created/updated timestamps
            var now = DateTime.UtcNow;
            
            // Calculate discount dates in UTC
            var discountStartDate = now.AddDays(-5);
            var discountEndDate = now.AddDays(10);

        //     // Seed some books
        //     modelBuilder.Entity<Book>().HasData(
        //         new Book
        //         {
        //             Id = 1,
        //             Title = "The Great Gatsby",
        //             Author = "F. Scott Fitzgerald",
        //             ISBN = "9780743273565",
        //             Description = "A story of wealth, love, and the American Dream in the 1920s.",
        //             Price = 12.99m,
        //             Genre = "Classic",
        //             Language = "English",
        //             Format = "Paperback",
        //             Publisher = "Scribner",
        //             PublicationDate = new DateTime(1925, 4, 10),
        //             StockQuantity = 50,
        //             CoverImageUrl = "/images/great-gatsby.jpg",
        //             AverageRating = 4.5,
        //             CreatedAt = now,
        //             UpdatedAt = now
        //         },
        //         new Book
        //         {
        //             Id = 2,
        //             Title = "To Kill a Mockingbird",
        //             Author = "Harper Lee",
        //             ISBN = "9780061120084",
        //             Description = "A powerful story of racial injustice and moral growth in the American South.",
        //             Price = 14.99m,
        //             Genre = "Classic",
        //             Language = "English",
        //             Format = "Paperback",
        //             Publisher = "HarperCollins",
        //             PublicationDate = new DateTime(1960, 7, 11),
        //             StockQuantity = 45,
        //             CoverImageUrl = "/images/to-kill-a-mockingbird.jpg",
        //             AverageRating = 4.8,
        //             CreatedAt = now,
        //             UpdatedAt = now
        //         },
        //         new Book
        //         {
        //             Id = 3,
        //             Title = "1984",
        //             Author = "George Orwell",
        //             ISBN = "9780451524935",
        //             Description = "A dystopian novel about totalitarianism, surveillance, and thought control.",
        //             Price = 11.99m,
        //             Genre = "Dystopian",
        //             Language = "English",
        //             Format = "Paperback",
        //             Publisher = "Signet Classic",
        //             PublicationDate = new DateTime(1949, 6, 8),
        //             StockQuantity = 60,
        //             CoverImageUrl = "/images/1984.jpg",
        //             AverageRating = 4.7,
        //             CreatedAt = now,
        //             UpdatedAt = now
        //         },
        //         new Book
        //         {
        //             Id = 4,
        //             Title = "Pride and Prejudice",
        //             Author = "Jane Austen",
        //             ISBN = "9780141439518",
        //             Description = "A romantic novel about the Bennet family and the proud Mr. Darcy.",
        //             Price = 9.99m,
        //             Genre = "Romance",
        //             Language = "English",
        //             Format = "Paperback",
        //             Publisher = "Penguin Classics",
        //             PublicationDate = new DateTime(1813, 1, 28),
        //             StockQuantity = 40,
        //             CoverImageUrl = "/images/pride-and-prejudice.jpg",
        //             AverageRating = 4.6,
        //             CreatedAt = now,
        //             UpdatedAt = now
        //         },
        //         new Book
        //         {
        //             Id = 5,
        //             Title = "The Hobbit",
        //             Author = "J.R.R. Tolkien",
        //             ISBN = "9780547928227",
        //             Description = "A fantasy novel about the adventures of Bilbo Baggins.",
        //             Price = 13.99m,
        //             Genre = "Fantasy",
        //             Language = "English",
        //             Format = "Paperback",
        //             Publisher = "Houghton Mifflin Harcourt",
        //             PublicationDate = new DateTime(1937, 9, 21),
        //             StockQuantity = 55,
        //             CoverImageUrl = "/images/the-hobbit.jpg",
        //             AverageRating = 4.9,
        //             IsOnSale = true,
        //             DiscountPrice = 10.99m,
        //             DiscountStartDate = discountStartDate,
        //             DiscountEndDate = discountEndDate,
        //             CreatedAt = now,
        //             UpdatedAt = now
        //         }
        //     );
         }

        // Helper method to create password hash
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
