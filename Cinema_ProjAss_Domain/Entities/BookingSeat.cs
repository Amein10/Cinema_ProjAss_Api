using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Domain.Entities
{
    /// <summary>
    /// Junction-entity der forbinder Booking og Seat.
    /// Bruges til at håndtere mange-til-mange-relationen mellem bookinger og sæder.
    /// </summary>
    public class BookingSeat
    {
        /// <summary>
        /// Fremmednøgle til Booking.
        /// </summary>
        public int BookingId { get; set; }

        /// <summary>
        /// Navigation til booking.
        /// </summary>
        public Booking Booking { get; set; } = null!;

        /// <summary>
        /// Fremmednøgle til Seat.
        /// </summary>
        public int SeatId { get; set; }

        /// <summary>
        /// Navigation til sæde.
        /// </summary>
        public Seat Seat { get; set; } = null!;

        /// <summary>
        /// Prisen for sædet på booking-tidspunktet.
        /// Bruges for at låse prisen historisk.
        /// </summary>
        public decimal PriceAtBooking { get; set; }
    }

}
