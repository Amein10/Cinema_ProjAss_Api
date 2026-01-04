using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Domain.Entities
{
    /// <summary>
    /// Repræsenterer en fysisk biografsal.
    /// En sal indeholder sæder og bruges til afholdelse af filmvisninger (shows).
    /// </summary>
    public class Auditorium
    {
        /// <summary>
        /// Primær nøgle for auditoriet.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Navn på salen, fx "Sal 1" eller "IMAX".
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Alle sæder der tilhører denne sal.
        /// </summary>
        public ICollection<Seat> Seats { get; set; } = new List<Seat>();

        /// <summary>
        /// Alle visninger (shows) der afholdes i denne sal.
        /// </summary>
        public ICollection<Show> Shows { get; set; } = new List<Show>();
    }


}
