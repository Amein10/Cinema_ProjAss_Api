using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema_ProjAss_Application.DTOs.Bookings;
using Cinema_ProjAss_Application.Exceptions;
using Cinema_ProjAss_Domain.Entities;
using Cinema_ProjAss_Domain.Interfaces;

namespace Cinema_ProjAss_Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookings;
        private readonly IShowRepository _shows;

        public BookingService(IBookingRepository bookings, IShowRepository shows)
        {
            _bookings = bookings;
            _shows = shows;
        }

        public async Task<BookingDto> GetByIdAsync(int id)
        {
            var booking = await _bookings.GetByIdAsync(id);
            if (booking == null) throw new NotFoundException($"Booking with id={id} not found.");
            return MapToDto(booking);
        }

        public async Task<IEnumerable<BookingDto>> GetForUserAsync(string userId)
        {
            userId = userId?.Trim() ?? string.Empty;
            if (userId.Length == 0) throw new ValidationException("userId is required.");

            var list = await _bookings.GetBookingsForUserAsync(userId);
            return list.Select(MapToDto);
        }

        public async Task<BookingDto> CreateAsync(CreateBookingDto dto)
        {
            ValidateCreate(dto);

            var show = await _shows.GetByIdAsync(dto.ShowId);
            if (show == null) throw new ValidationException($"ShowId={dto.ShowId} does not exist.");

            var booking = new Booking
            {
                UserId = dto.UserId.Trim(),
                ShowId = dto.ShowId,
                Status = BookingStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                BookingSeats = dto.SeatIds
                    .Distinct()
                    .Select(seatId => new BookingSeat
                    {
                        SeatId = seatId,
                        PriceAtBooking = show.Price
                    })
                    .ToList(),
                Show = show
            };

            var created = await _bookings.CreateAsync(booking);

            var loaded = await _bookings.GetByIdAsync(created.Id);
            if (loaded == null) throw new NotFoundException("Booking was created but could not be loaded.");

            return MapToDto(loaded);
        }

        public async Task UpdateStatusAsync(int bookingId, string status)
        {
            if (bookingId <= 0) throw new ValidationException("bookingId must be > 0.");
            if (string.IsNullOrWhiteSpace(status)) throw new ValidationException("status is required.");

            if (!Enum.TryParse<BookingStatus>(status, ignoreCase: true, out var parsed))
                throw new ValidationException("Invalid status. Use Pending, Confirmed, or Cancelled.");

            var existing = await _bookings.GetByIdAsync(bookingId);
            if (existing == null) throw new NotFoundException($"Booking with id={bookingId} not found.");

            await _bookings.UpdateStatusAsync(bookingId, parsed);
        }

        // -------- NEW: PUT (Update seats) --------
        public async Task UpdateAsync(int bookingId, UpdateBookingDto dto)
        {
            if (bookingId <= 0) throw new ValidationException("bookingId must be > 0.");
            if (dto == null) throw new ValidationException("Body is required.");
            if (dto.SeatIds == null || dto.SeatIds.Count == 0) throw new ValidationException("At least one SeatId is required.");
            if (dto.SeatIds.Any(id => id <= 0)) throw new ValidationException("SeatIds must be > 0.");

            var existing = await _bookings.GetByIdAsync(bookingId);
            if (existing == null) throw new NotFoundException($"Booking with id={bookingId} not found.");

            await _bookings.UpdateSeatsAsync(bookingId, dto.SeatIds);

            // Returnér opdateret version
            var loaded = await _bookings.GetByIdAsync(bookingId);
            if (loaded == null) throw new NotFoundException("Booking was updated but could not be loaded.");

            // (Controller kan vælge NoContent, men du kan også returne DTO i controller)
        }

        // -------- NEW: DELETE --------
        public async Task DeleteAsync(int bookingId)
        {
            if (bookingId <= 0) throw new ValidationException("bookingId must be > 0.");

            var existing = await _bookings.GetByIdAsync(bookingId);
            if (existing == null) throw new NotFoundException($"Booking with id={bookingId} not found.");

            await _bookings.DeleteAsync(bookingId);
        }

        private static void ValidateCreate(CreateBookingDto dto)
        {
            if (dto == null) throw new ValidationException("Body is required.");
            if (string.IsNullOrWhiteSpace(dto.UserId)) throw new ValidationException("UserId is required.");
            if (dto.ShowId <= 0) throw new ValidationException("ShowId must be > 0.");
            if (dto.SeatIds == null || dto.SeatIds.Count == 0) throw new ValidationException("At least one SeatId is required.");
            if (dto.SeatIds.Any(id => id <= 0)) throw new ValidationException("SeatIds must be > 0.");
        }

        private static BookingDto MapToDto(Booking b)
        {
            return new BookingDto
            {
                Id = b.Id,
                UserId = b.UserId,
                ShowId = b.ShowId,
                ShowStartTime = b.Show?.StartTime ?? default,
                MovieTitle = b.Show?.Movie?.Title ?? string.Empty,
                AuditoriumName = b.Show?.Auditorium?.Name ?? string.Empty,
                CreatedAt = b.CreatedAt,
                Status = b.Status.ToString(),
                Seats = b.BookingSeats.Select(bs => new BookingSeatDto
                {
                    SeatId = bs.SeatId,
                    Row = bs.Seat?.Row ?? string.Empty,
                    Number = bs.Seat?.Number ?? 0,
                    PriceAtBooking = bs.PriceAtBooking
                }).ToList(),
                PaymentAmount = b.Payment?.Amount,
                PaymentStatus = b.Payment?.Status.ToString(),
                PaymentMethod = b.Payment?.Method.ToString()
            };
        }
    }
}
