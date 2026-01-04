using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Application.DTOs.Bookings
{
    /// <summary>
    /// Output-model for en booking inkl. show-info, sæder og evt. payment.
    /// </summary>
    public class BookingDto
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public int ShowId { get; set; }

        public DateTime ShowStartTime { get; set; }

        public string MovieTitle { get; set; } = string.Empty;

        public string AuditoriumName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// BookingStatus som tekst (Pending/Confirmed/Cancelled).
        /// </summary>
        public string Status { get; set; } = string.Empty;

        public List<BookingSeatDto> Seats { get; set; } = new();

        public decimal? PaymentAmount { get; set; }
        public string? PaymentStatus { get; set; }
        public string? PaymentMethod { get; set; }
    }
}
