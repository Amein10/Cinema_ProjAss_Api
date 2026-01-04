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

        // userId kommer fra JWT (AppUser.Id)
        Task<BookingDto> CreateAsync(int userId, CreateBookingDto dto);

        Task UpdateStatusAsync(int bookingId, string status);
    }
}