using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Application.DTOs.Bookings
{
    /// <summary>
    /// Output-model for et sæde i en booking.
    /// </summary>
    public class BookingSeatDto
    {
        public int SeatId { get; set; }

        public string Row { get; set; } = string.Empty;

        public int Number { get; set; }

        public decimal PriceAtBooking { get; set; }
    }
}
