using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDiscountService _discountService;
        private const decimal TAX_RATE = 0.08m; // 8%

        public CartService(ApplicationDbContext context, IDiscountService discountService)
        {
            _context = context;
            _discountService = discountService;
        }

        public async Task<CartSummaryDto> GetCartAsync(int userId)
        {
            var cartItems = await _context.Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.Book)
                .Select(c => new CartItemDto
                {
                    Id = c.Id,
                    BookId = c.BookId,
                    Title = c.Book.Title,
                    Author = c.Book.Author,
                    CoverImageUrl = c.Book.CoverImageUrl,
                    Price = c.Book.Price,
                    IsOnSale = c.Book.IsOnSale,
                    DiscountPrice = c.Book.DiscountPrice,
                    DiscountStartDate = c.Book.DiscountStartDate,
                    DiscountEndDate = c.Book.DiscountEndDate,
                    Quantity = c.Quantity,
                    StockQuantity = c.Book.StockQuantity
                })
                .ToListAsync();

            var subtotal = cartItems.Sum(item => item.Subtotal);
            int totalItems = cartItems.Sum(item => item.Quantity);
            
            // Calculate discounts
            var discounts = await _discountService.CalculateDiscountsAsync(userId, subtotal, totalItems);
            
            // Calculate tax on discounted subtotal
            var tax = discounts.DiscountedSubtotal * TAX_RATE;
            var total = discounts.DiscountedSubtotal + tax;

            // Debug logging
            Console.WriteLine($"Cart Summary - User: {userId}, Items: {totalItems}, Subtotal: ${subtotal}");
            Console.WriteLine($"Bulk Discount: {discounts.HasBulkDiscount}, Amount: ${discounts.BulkDiscountAmount}");
            Console.WriteLine($"Loyalty Discount: {discounts.HasLoyaltyDiscount}, Amount: ${discounts.LoyaltyDiscountAmount}");
            Console.WriteLine($"Total Discount: ${discounts.DiscountAmount}, Discounted Subtotal: ${discounts.DiscountedSubtotal}");

            return new CartSummaryDto
            {
                Items = cartItems,
                TotalItems = totalItems,
                Subtotal = subtotal,
                DiscountAmount = discounts.DiscountAmount,
                DiscountedSubtotal = discounts.DiscountedSubtotal,
                HasBulkDiscount = discounts.HasBulkDiscount,
                HasLoyaltyDiscount = discounts.HasLoyaltyDiscount,
                BulkDiscountAmount = discounts.BulkDiscountAmount,
                LoyaltyDiscountAmount = discounts.LoyaltyDiscountAmount,
                DiscountDescription = discounts.DiscountDescription,
                Tax = tax,
                Total = total
            };
        }

        public async Task<CartItemDto> AddToCartAsync(int userId, AddToCartDto dto)
        {
            // Check if the book exists
            var book = await _context.Books.FindAsync(dto.BookId);
            if (book == null)
            {
                throw new Exception("Book not found");
            }

            // Check if the book is in stock
            if (book.StockQuantity < dto.Quantity)
            {
                throw new Exception("Not enough stock available");
            }

            // Check if the book is already in the cart
            var existingCartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.BookId == dto.BookId);

            if (existingCartItem != null)
            {
                // Update the quantity
                existingCartItem.Quantity += dto.Quantity;
                existingCartItem.UpdatedAt = DateTime.UtcNow;
                
                // Check if the new quantity exceeds the stock
                if (existingCartItem.Quantity > book.StockQuantity)
                {
                    throw new Exception("Not enough stock available");
                }
                
                await _context.SaveChangesAsync();

                return new CartItemDto
                {
                    Id = existingCartItem.Id,
                    BookId = existingCartItem.BookId,
                    Title = book.Title,
                    Author = book.Author,
                    CoverImageUrl = book.CoverImageUrl,
                    Price = book.Price,
                    IsOnSale = book.IsOnSale,
                    DiscountPrice = book.DiscountPrice,
                    DiscountStartDate = book.DiscountStartDate,
                    DiscountEndDate = book.DiscountEndDate,
                    Quantity = existingCartItem.Quantity,
                    StockQuantity = book.StockQuantity
                };
            }
            else
            {
                // Add new cart item
                var cartItem = new Cart
                {
                    UserId = userId,
                    BookId = dto.BookId,
                    Quantity = dto.Quantity,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _context.Carts.AddAsync(cartItem);
                await _context.SaveChangesAsync();

                return new CartItemDto
                {
                    Id = cartItem.Id,
                    BookId = cartItem.BookId,
                    Title = book.Title,
                    Author = book.Author,
                    CoverImageUrl = book.CoverImageUrl,
                    Price = book.Price,
                    IsOnSale = book.IsOnSale,
                    DiscountPrice = book.DiscountPrice,
                    DiscountStartDate = book.DiscountStartDate,
                    DiscountEndDate = book.DiscountEndDate,
                    Quantity = cartItem.Quantity,
                    StockQuantity = book.StockQuantity
                };
            }
        }

        public async Task<CartItemDto> UpdateCartItemAsync(int userId, int cartItemId, UpdateCartItemDto dto)
        {
            // Find the cart item
            var cartItem = await _context.Carts
                .Include(c => c.Book)
                .FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);

            if (cartItem == null)
            {
                throw new Exception("Cart item not found");
            }

            // Check if the new quantity exceeds the stock
            if (dto.Quantity > cartItem.Book.StockQuantity)
            {
                throw new Exception("Not enough stock available");
            }

            // Update the quantity
            cartItem.Quantity = dto.Quantity;
            cartItem.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new CartItemDto
            {
                Id = cartItem.Id,
                BookId = cartItem.BookId,
                Title = cartItem.Book.Title,
                Author = cartItem.Book.Author,
                CoverImageUrl = cartItem.Book.CoverImageUrl,
                Price = cartItem.Book.Price,
                IsOnSale = cartItem.Book.IsOnSale,
                DiscountPrice = cartItem.Book.DiscountPrice,
                DiscountStartDate = cartItem.Book.DiscountStartDate,
                DiscountEndDate = cartItem.Book.DiscountEndDate,
                Quantity = cartItem.Quantity,
                StockQuantity = cartItem.Book.StockQuantity
            };
        }

        public async Task RemoveFromCartAsync(int userId, int cartItemId)
        {
            var cartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);

            if (cartItem != null)
            {
                _context.Carts.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(int userId)
        {
            var cartItems = await _context.Carts
                .Where(c => c.UserId == userId)
                .ToListAsync();

            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
    }
}
