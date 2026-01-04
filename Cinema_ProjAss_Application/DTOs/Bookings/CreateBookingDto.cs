using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Application.DTOs.Bookings
{
    /// <summary>
    /// Input-model til at oprette en booking.
    /// UserId kommer fra JWT token (ikke fra client).
    /// </summary>
    public class CreateBookingDto
    {
        public int ShowId { get; set; }

        public List<int> SeatIds { get; set; } = new();
    }
}