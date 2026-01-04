using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema_ProjAss_Domain.Entities;

namespace Cinema_ProjAss_Domain.Interfaces
{
    /// <summary>
    /// Repository-kontrakt for håndtering af bookinger.
    /// Indeholder metoder til at hente, oprette og opdatere booking-status.
    /// </summary>
    public interface IBookingRepository
    {
        /// <summary>
        /// Henter en booking ud fra dens id.
        /// Returnerer null hvis bookingen ikke findes.
        /// </summary>
        /// <param name="id">Bookingens id.</param>
        /// <returns>Booking eller null.</returns>
        Task<Booking?> GetByIdAsync(int id);

        /// <summary>
        /// Henter alle bookinger for en given bruger.
        /// Bruges fx til "Mine bookinger"-oversigt.
        /// </summary>
        /// <param name="userId">Brugerens id (fx fra Identity).</param>
        /// <returns>Liste af bookinger for brugeren.</returns>
        Task<IEnumerable<Booking>> GetBookingsForUserAsync(string userId);

        /// <summary>
        /// Opretter en ny booking i systemet.
        /// Forventer at bookingen indeholder tilhørende sæder (BookingSeats) efter behov.
        /// </summary>
        /// <param name="booking">Booking-objektet der skal oprettes.</param>
        /// <returns>Den oprettede booking (typisk inkl. genereret Id).</returns>
        Task<Booking> CreateAsync(Booking booking);

        /// <summary>
        /// Opdaterer status på en eksisterende booking (fx Pending -> Confirmed eller Cancelled).
        /// </summary>
        /// <param name="bookingId">Id på bookingen der skal opdateres.</param>
        /// <param name="status">Ny status.</param>
        Task UpdateStatusAsync(int bookingId, BookingStatus status);
    }
}

