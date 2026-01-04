using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Domain.Entities
{
    /// <summary>
    /// Repræsenterer en filmvisning på et bestemt tidspunkt i en bestemt sal.
    /// </summary>
    public class Show
    {
        /// <summary>
        /// Primær nøgle for visningen.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Starttidspunkt for visningen.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Pris pr. billet for denne visning.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Fremmednøgle til filmen der vises.
        /// </summary>
        public int MovieId { get; set; }

        /// <summary>
        /// Navigation til filmen.
        /// </summary>
        public Movie Movie { get; set; } = null!;

        /// <summary>
        /// Fremmednøgle til auditoriet hvor visningen foregår.
        /// </summary>
        public int AuditoriumId { get; set; }

        /// <summary>
        /// Navigation til auditorium.
        /// </summary>
        public Auditorium Auditorium { get; set; } = null!;

        /// <summary>
        /// Bookinger foretaget til denne visning.
        /// </summary>
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }

}
