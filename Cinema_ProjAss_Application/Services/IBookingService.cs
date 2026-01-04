using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema_ProjAss_Application.DTOs.Bookings;

namespace Cinema_ProjAss_Application.Services
{
    public interface IBookingService
    {
        Task<BookingDto> GetByIdAsync(int id);
        Task<IEnumerable<BookingDto>> GetForUserAsync(string userId);

        Task<BookingDto> CreateAsync(CreateBookingDto dto);

        // Ikke-basic
        Task UpdateStatusAsync(int bookingId, string status);

        // CRUD
        Task UpdateAsync(int bookingId, UpdateBookingDto dto);
        Task DeleteAsync(int bookingId);
    }
}
