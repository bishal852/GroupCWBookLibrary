using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.DTOs;
using backend.Models;
using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;

namespace backend.Services
{
    public class BannerService : IBannerService
    {
        private readonly ApplicationDbContext _context;
        // private readonly ILogger<BannerService> _logger;

        public BannerService(ApplicationDbContext context /*, ILogger<BannerService> logger */)
        {
            _context = context;
            // _logger = logger;
        }

        public async Task<List<BannerDto>> GetAllBannersAsync()
        {
            var banners = await _context.Banners
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return banners.Select(MapToDto).ToList();
        }

        public async Task<BannerDto> GetBannerByIdAsync(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            return banner == null ? null : MapToDto(banner);
        }

        public async Task<BannerDto> GetActiveBannerAsync()
        {
            var now = DateTime.UtcNow;
            var activeBanner = await _context.Banners
                .Where(b => b.StartDate <= now && b.EndDate >= now)
                .OrderByDescending(b => b.CreatedAt)
                .FirstOrDefaultAsync();

            return activeBanner == null ? null : MapToDto(activeBanner);
        }

        public async Task<BannerDto> CreateBannerAsync(CreateBannerDto createBannerDto)
        {
            if (createBannerDto.EndDate <= createBannerDto.StartDate)
                throw new ArgumentException("End date must be after start date");

            var banner = new Banner
            {
                Message = createBannerDto.Message,
                StartDate = createBannerDto.StartDate.ToUniversalTime(),
                EndDate = createBannerDto.EndDate.ToUniversalTime(),
                BackgroundColor = createBannerDto.BackgroundColor,
                TextColor = createBannerDto.TextColor,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Banners.Add(banner);
            await _context.SaveChangesAsync();

            return MapToDto(banner);
        }

        public async Task<BannerDto> UpdateBannerAsync(int id, UpdateBannerDto updateBannerDto)
        {
            if (updateBannerDto.EndDate <= updateBannerDto.StartDate)
                throw new ArgumentException("End date must be after start date");

            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
                return null;

            banner.Message = updateBannerDto.Message;
            banner.StartDate = updateBannerDto.StartDate.ToUniversalTime();
            banner.EndDate = updateBannerDto.EndDate.ToUniversalTime();
            banner.BackgroundColor = updateBannerDto.BackgroundColor;
            banner.TextColor = updateBannerDto.TextColor;
            banner.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToDto(banner);
        }

        public async Task<bool> DeleteBannerAsync(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
                return false;

            _context.Banners.Remove(banner);
            await _context.SaveChangesAsync();

            return true;
        }

        private BannerDto MapToDto(Banner banner)
        {
            var now = DateTime.UtcNow;

            return new BannerDto
            {
                Id = banner.Id,
                Message = banner.Message,
                StartDate = banner.StartDate,
                EndDate = banner.EndDate,
                BackgroundColor = banner.BackgroundColor,
                TextColor = banner.TextColor,
                IsActive = banner.StartDate <= now && banner.EndDate >= now,
                CreatedAt = banner.CreatedAt,
                UpdatedAt = banner.UpdatedAt
            };
        }
    }
}
