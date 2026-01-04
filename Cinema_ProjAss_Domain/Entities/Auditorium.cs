using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Domain.Entities
{
    public class Auditorium
    {
        public int Id { get; set; }

        // fx "Sal 1"
        public string Name { get; set; } = string.Empty;

        // Navigation
        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
        public ICollection<Show> Shows { get; set; } = new List<Show>();
    }
}
