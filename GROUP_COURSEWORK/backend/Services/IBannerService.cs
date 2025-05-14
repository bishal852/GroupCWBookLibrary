using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs;

namespace backend.Services
{
    public interface IBannerService
    {
        Task<List<BannerDto>> GetAllBannersAsync();
        Task<BannerDto> GetBannerByIdAsync(int id);
        Task<BannerDto> GetActiveBannerAsync();
        Task<BannerDto> CreateBannerAsync(CreateBannerDto createBannerDto);
        Task<BannerDto> UpdateBannerAsync(int id, UpdateBannerDto updateBannerDto);
        Task<bool> DeleteBannerAsync(int id);
    }
}
