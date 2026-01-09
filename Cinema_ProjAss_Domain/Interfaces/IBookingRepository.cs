using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema_ProjAss_Domain.Entities;

namespace Cinema_ProjAss_Domain.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(int id);
        Task<IEnumerable<Booking>> GetBookingsForUserAsync(string userId);

        Task<Booking> CreateAsync(Booking booking);

        // Ikke-basic opdatering (du har allerede)
        Task UpdateStatusAsync(int bookingId, BookingStatus status);

        // CRUD mangler:
        Task UpdateSeatsAsync(int bookingId, List<int> seatIds);
        Task DeleteAsync(int bookingId);
        Task<List<int>> GetBookedSeatIdsForShowAsync(int showId);
        Task<bool> AnyBookedSeatsAsync(int showId, IEnumerable<int> seatIds);

    }
}
