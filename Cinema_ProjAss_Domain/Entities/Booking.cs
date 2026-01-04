using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Domain.Entities
{
    /// <summary>
    /// Repræsenterer en booking foretaget af en bruger til en bestemt visning.
    /// En booking kan indeholde flere sæder og evt. en betaling.
    /// </summary>
    public class Booking
    {
        /// <summary>
        /// Primær nøgle for bookingen.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID på den bruger der har foretaget bookingen.
        /// Senere kan denne kobles til ASP.NET Identity.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Reference til den visning (show) som bookingen gælder for.
        /// </summary>
        public int ShowId { get; set; }

        /// <summary>
        /// Navigation til den bookede visning.
        /// </summary>
        public Show Show { get; set; } = null!;

        /// <summary>
        /// Tidspunkt hvor bookingen blev oprettet (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Status for bookingen (fx Pending, Confirmed, Cancelled).
        /// </summary>
        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        /// <summary>
        /// De sæder der er inkluderet i bookingen.
        /// </summary>
        public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();

        /// <summary>
        /// Betaling knyttet til bookingen (kan være null hvis ikke betalt endnu).
        /// </summary>
        public Payment? Payment { get; set; }
    }
}
