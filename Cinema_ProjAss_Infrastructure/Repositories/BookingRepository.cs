using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema_ProjAss_Domain.Entities;
using Cinema_ProjAss_Domain.Interfaces;
using Cinema_ProjAss_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cinema_ProjAss_Infrastructure.Repositories
{
    /// <summary>
    /// EF Core implementation af IBookingRepository.
    /// Håndterer persistence for Booking samt eager loading af relaterede data.
    /// </summary>
    public class BookingRepository : IBookingRepository
    {
        private readonly CinemaDbContext _context;

        /// <summary>
        /// Opretter repository med DbContext via dependency injection.
        /// </summary>
        public BookingRepository(CinemaDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Show)
                    .ThenInclude(s => s.Movie)
                .Include(b => b.Show)
                    .ThenInclude(s => s.Auditorium)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.Seat)
                .Include(b => b.Payment)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Booking>> GetBookingsForUserAsync(string userId)
        {
            return await _context.Bookings
                .Include(b => b.Show)
                    .ThenInclude(s => s.Movie)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.Seat)
                .Include(b => b.Payment)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Booking> CreateAsync(Booking booking)
        {
            // Bemærk:
            // Hvis booking.Show ikke er loaded, kan du alternativt sætte PriceAtBooking i service-laget
            // ved at slå Show.Price op via repository før CreateAsync kaldes.

            if (booking.BookingSeats != null && booking.BookingSeats.Count > 0)
            {
                foreach (var bs in booking.BookingSeats)
                {
                    // Hvis PriceAtBooking ikke er sat, forsøger vi at låse show-prisen.
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

        /// <inheritdoc />
        public async Task UpdateStatusAsync(int bookingId, BookingStatus status)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null) return;

            booking.Status = status;
            await _context.SaveChangesAsync();
        }
    }
}
