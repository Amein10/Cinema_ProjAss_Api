using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Domain.Entities
{
    public class BookingSeat
    {
        // Junction keys
        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        public int SeatId { get; set; }
        public Seat Seat { get; set; } = null!;

        // “B”-delen: pris låses på booking-tidspunktet
        public decimal PriceAtBooking { get; set; }
    }
}
