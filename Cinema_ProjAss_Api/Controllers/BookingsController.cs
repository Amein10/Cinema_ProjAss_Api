using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cinema_ProjAss_Application.DTOs.Bookings;
using Cinema_ProjAss_Application.Services;

namespace Cinema_ProjAss_Api.Controllers
{
    /// <summary>
    /// Bookings endpoints kræver login.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookingDto>> GetById(int id)
        {
            var booking = await _bookingService.GetByIdAsync(id);
            return Ok(booking);
        }

        /// <summary>
        /// Henter bookinger for den bruger der er logget ind.
        /// </summary>
        [HttpGet("me")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetMineBookings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bookings = await _bookingService.GetForUserAsync(userId ?? "");
            return Ok(bookings);
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> Create([FromBody] CreateBookingDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdStr, out var userId))
                return Unauthorized("Invalid token user id.");

            var created = await _bookingService.CreateAsync(userId, dto);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PATCH api/bookings/5/status?status=Confirmed
        // (Du kan vælge at beholde den offentlig eller senere lave admin-rolle)
        [HttpPatch("{bookingId:int}/status")]
        public async Task<IActionResult> UpdateStatus(int bookingId, [FromQuery] string status)
        {
            await _bookingService.UpdateStatusAsync(bookingId, status);
            return NoContent();
        }
    }
}
