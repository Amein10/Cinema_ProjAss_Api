using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }

        // Hvem booker (senere kobler vi til Identity User)
        public string UserId { get; set; } = string.Empty;

        // Hvilken visning bookes der til?
        public int ShowId { get; set; }
        public Show Show { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        // Navigation: hvilke sæder er med i bookingen
        public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();

        public Payment? Payment { get; set; }

    }
}
