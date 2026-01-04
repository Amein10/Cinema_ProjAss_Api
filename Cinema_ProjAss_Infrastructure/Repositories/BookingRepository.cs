using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cinema_ProjAss_Domain.Entities;
using Cinema_ProjAss_Domain.Interfaces;
using Cinema_ProjAss_Infrastructure.Data;

namespace Cinema_ProjAss_Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly CinemaDbContext _context;

        public BookingRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Show).ThenInclude(s => s.Movie)
                .Include(b => b.Show).ThenInclude(s => s.Auditorium)
                .Include(b => b.BookingSeats).ThenInclude(bs => bs.Seat)
                .Include(b => b.Payment)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Booking>> GetBookingsForUserAsync(string userId)
        {
            return await _context.Bookings
                .Include(b => b.Show).ThenInclude(s => s.Movie)
                .Include(b => b.Show).ThenInclude(s => s.Auditorium)
                .Include(b => b.BookingSeats).ThenInclude(bs => bs.Seat)
                .Include(b => b.Payment)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<Booking> CreateAsync(Booking booking)
        {
            // Sæt PriceAtBooking hvis ikke sat
            if (booking.BookingSeats != null && booking.BookingSeats.Count > 0)
            {
                foreach (var bs in booking.BookingSeats)
                {
                    if (bs.PriceAtBooking <= 0)
                    {
                        bs.PriceAtBooking = booking.Show?.Price ?? bs.PriceAtBooking;
                    }
                }
            }

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task UpdateStatusAsync(int bookingId, BookingStatus status)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null) return;

            booking.Status = status;
            await _context.SaveChangesAsync();
        }

        // --------- NEW: PUT (Update seats) ----------
        public async Task UpdateSeatsAsync(int bookingId, List<int> seatIds)
        {
            var booking = await _context.Bookings
                .Include(b => b.Show)
                .Include(b => b.BookingSeats)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null) return;

            var distinctSeatIds = seatIds.Distinct().ToList();

            // Optional sanity: sikre seats findes (ellers FK fejl)
            var existingSeatsCount = await _context.Seats.CountAsync(s => distinctSeatIds.Contains(s.Id));
            if (existingSeatsCount != distinctSeatIds.Count)
            {
                // Nogle seatIds findes ikke -> vi lader service håndtere (throw der) eller gør intet her.
                // Her vælger vi bare at lade service validere før kald.
            }

            // Fjern gamle bookingSeats
            _context.BookingSeats.RemoveRange(booking.BookingSeats);

            // Tilføj nye bookingSeats
            booking.BookingSeats = distinctSeatIds
                .Select(seatId => new BookingSeat
                {
                    BookingId = booking.Id,
                    SeatId = seatId,
                    PriceAtBooking = booking.Show?.Price ?? 0m
                })
                .ToList();

            await _context.SaveChangesAsync();
        }

        // --------- NEW: DELETE (Hard delete) ----------
        public async Task DeleteAsync(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.BookingSeats)
                .Include(b => b.Payment)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null) return;

            // Fjern Payment først (for at undgå FK problemer)
            if (booking.Payment != null)
            {
                _context.Payments.Remove(booking.Payment);
            }

            // Fjern junction rows
            _context.BookingSeats.RemoveRange(booking.BookingSeats);

            // Fjern booking
            _context.Bookings.Remove(booking);

            await _context.SaveChangesAsync();
        }
    }
}
