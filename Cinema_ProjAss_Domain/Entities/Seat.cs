using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Domain.Entities
{
    public class Seat
    {
        public int Id { get; set; }

        // fx "A", "B", "C"
        public string Row { get; set; } = string.Empty;

        // fx 1,2,3...
        public int Number { get; set; }

        // Relation til Auditorium (1 sal -> mange seats)
        public int AuditoriumId { get; set; }
        public Auditorium Auditorium { get; set; } = null!;
        public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();

    }
}
