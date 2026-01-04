using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Domain.Entities
{
    /// <summary>
    /// Repræsenterer et sæde i en biografsal.
    /// </summary>
    public class Seat
    {
        /// <summary>
        /// Primær nøgle for sædet.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Række-betegnelse, fx "A", "B" eller "C".
        /// </summary>
        public string Row { get; set; } = string.Empty;

        /// <summary>
        /// Sædets nummer inden for rækken.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Fremmednøgle til det auditorium sædet tilhører.
        /// </summary>
        public int AuditoriumId { get; set; }

        /// <summary>
        /// Navigation til auditorium.
        /// </summary>
        public Auditorium Auditorium { get; set; } = null!;

        /// <summary>
        /// Bookinger der inkluderer dette sæde.
        /// </summary>
        public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();
    }

}
