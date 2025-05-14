using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.DTOs;
using backend.Services;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Staff")]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;
        private readonly IEmailService _emailService;

        public StaffController(IStaffService staffService, IEmailService emailService)
        {
            _staffService = staffService;
            _emailService = emailService;
        }

        [HttpGet("order/{claimCode}")]
        public async Task<IActionResult> GetOrderByClaimCode(string claimCode)
        {
            try
            {
                var order = await _staffService.GetOrderByClaimCodeAsync(claimCode);
                
                if (order == null)
                {
                    return NotFound(new { message = "Order not found with the provided claim code" });
                }
                
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("fulfill")]
        public async Task<IActionResult> FulfillOrder([FromBody] ClaimCodeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var staffId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var order = await _staffService.FulfillOrderByClaimCodeAsync(staffId, dto.ClaimCode);
                
                // Log successful fulfillment
                Console.WriteLine($"Order {order.Id} fulfilled successfully by staff {staffId}");
                
                return Ok(order);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fulfilling order: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
