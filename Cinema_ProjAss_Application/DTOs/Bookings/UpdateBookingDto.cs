using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Application.DTOs.Bookings
{
    public class UpdateBookingDto
    {
        // Vi opdaterer kun sæder (det giver mening i booking-flow)
        public List<int> SeatIds { get; set; } = new();
    }
}
