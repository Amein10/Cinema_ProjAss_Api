using Cinema_ProjAss_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Domain.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(int id);

        Task<IEnumerable<Booking>> GetBookingsForUserAsync(string userId);

        Task<Booking> CreateAsync(Booking booking);

        Task UpdateStatusAsync(int bookingId, BookingStatus status);
    }
}
