using System.Threading.Tasks;
using backend.DTOs;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _bannerService;

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBanners()
        {
            var banners = await _bannerService.GetAllBannersAsync();
            return Ok(banners);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBannerById(int id)
        {
            var banner = await _bannerService.GetBannerByIdAsync(id);
            if (banner == null)
            {
                return NotFound();
            }

            return Ok(banner);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveBanner()
        {
            var banner = await _bannerService.GetActiveBannerAsync();
            if (banner == null)
            {
                return NotFound();
            }

            return Ok(banner);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateBanner([FromBody] CreateBannerDto createBannerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var banner = await _bannerService.CreateBannerAsync(createBannerDto);
                return CreatedAtAction(nameof(GetBannerById), new { id = banner.Id }, banner);
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBanner(int id, [FromBody] UpdateBannerDto updateBannerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var banner = await _bannerService.UpdateBannerAsync(id, updateBannerDto);
                if (banner == null)
                {
                    return NotFound();
                }

                return Ok(banner);
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBanner(int id)
        {
            var result = await _bannerService.DeleteBannerAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
