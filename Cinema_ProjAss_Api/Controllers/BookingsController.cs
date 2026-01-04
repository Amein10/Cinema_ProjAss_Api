using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Cinema_ProjAss_Application.DTOs.Bookings;
using Cinema_ProjAss_Application.Exceptions;
using Cinema_ProjAss_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema_ProjAss_Api.Controllers
{
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

        private string GetUserIdOrThrow()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(id))
                throw new ValidationException("Missing user id claim.");
            return id;
        }

        // GET: /api/bookings/{id}
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingDto>> GetById(int id)
        {
            var booking = await _bookingService.GetByIdAsync(id);
            return Ok(booking);
        }

        // GET: /api/bookings/me
        [HttpGet("me")]
        [ProducesResponseType(typeof(IEnumerable<BookingDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetForMe()
        {
            var userId = GetUserIdOrThrow();
            var bookings = await _bookingService.GetForUserAsync(userId);
            return Ok(bookings);
        }

        // POST: /api/bookings
        [HttpPost]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookingDto>> Create([FromBody] CreateBookingDto dto)
        {
            // Overstyr userId fra token (så klienten ikke kan spoofe)
            dto.UserId = GetUserIdOrThrow();

            var created = await _bookingService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PATCH: /api/bookings/{bookingId}/status?status=Confirmed
        [HttpPatch("{bookingId:int}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStatus(int bookingId, [FromQuery] string status)
        {
            await _bookingService.UpdateStatusAsync(bookingId, status);
            return NoContent();
        }

        // PUT: /api/bookings/{bookingId}
        [HttpPut("{bookingId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int bookingId, [FromBody] UpdateBookingDto dto)
        {
            await _bookingService.UpdateAsync(bookingId, dto);
            return NoContent();
        }

        // DELETE: /api/bookings/{bookingId}
        [HttpDelete("{bookingId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int bookingId)
        {
            await _bookingService.DeleteAsync(bookingId);
            return NoContent();
        }
    }
}
