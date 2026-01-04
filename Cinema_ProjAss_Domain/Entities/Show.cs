using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Domain.Entities
{
    public class Show
    {
        public int Id { get; set; }

        // Hvornår visningen starter
        public DateTime StartTime { get; set; }

        // Pris pr. billet (kan ændres pr. show)
        public decimal Price { get; set; }

        // Relation til Movie (1 Movie -> mange Shows)
        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;

        public int AuditoriumId { get; set; }
        public Auditorium Auditorium { get; set; } = null!;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    }
}
