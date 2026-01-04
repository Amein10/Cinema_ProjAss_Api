using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cinema_ProjAss_Application.DTOs.Bookings;
using Cinema_ProjAss_Application.Exceptions;
using Cinema_ProjAss_Application.Services;

namespace Cinema_ProjAss_Api.Controllers
{
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
            try
            {
                var booking = await _bookingService.GetByIdAsync(id);
                return Ok(booking);
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetForUser(string userId)
        {
            try
            {
                var bookings = await _bookingService.GetForUserAsync(userId);
                return Ok(bookings);
            }
            catch (ValidationException ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> Create([FromBody] CreateBookingDto dto)
        {
            try
            {
                var created = await _bookingService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ValidationException ex) { return BadRequest(ex.Message); }
        }

        // Ikke-basic (business)
        [HttpPatch("{bookingId:int}/status")]
        public async Task<IActionResult> UpdateStatus(int bookingId, [FromQuery] string status)
        {
            try
            {
                await _bookingService.UpdateStatusAsync(bookingId, status);
                return NoContent();
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (ValidationException ex) { return BadRequest(ex.Message); }
        }

        // -------- NEW: PUT (Update) --------
        [HttpPut("{bookingId:int}")]
        public async Task<IActionResult> Update(int bookingId, [FromBody] UpdateBookingDto dto)
        {
            try
            {
                await _bookingService.UpdateAsync(bookingId, dto);
                return NoContent(); // eller Ok(await _bookingService.GetByIdAsync(bookingId));
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (ValidationException ex) { return BadRequest(ex.Message); }
        }

        // -------- NEW: DELETE (Delete) --------
        [HttpDelete("{bookingId:int}")]
        public async Task<IActionResult> Delete(int bookingId)
        {
            try
            {
                await _bookingService.DeleteAsync(bookingId);
                return NoContent();
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (ValidationException ex) { return BadRequest(ex.Message); }
        }
    }
}
