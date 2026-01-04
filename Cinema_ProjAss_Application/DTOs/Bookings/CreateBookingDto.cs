using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Application.DTOs.Bookings
{
    public class CreateBookingDto
    {
        public int ShowId { get; set; }
        public List<int> SeatIds { get; set; } = new();

        // Server-only:
        public string UserId { get; set; } = string.Empty;
    }

}
