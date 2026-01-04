using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Domain.Entities
{
    /// <summary>
    /// Repræsenterer en betaling knyttet til en booking.
    /// </summary>
    public class Payment
    {
        /// <summary>
        /// Primær nøgle for betalingen.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Fremmednøgle til den booking betalingen vedrører.
        /// </summary>
        public int BookingId { get; set; }

        /// <summary>
        /// Navigation til booking.
        /// </summary>
        public Booking Booking { get; set; } = null!;

        /// <summary>
        /// Det samlede beløb for betalingen.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Betalingsmetode (fx Card, MobilePay).
        /// </summary>
        public PaymentMethod Method { get; set; } = PaymentMethod.Unknown;

        /// <summary>
        /// Status for betalingen.
        /// </summary>
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        /// <summary>
        /// Tidspunkt hvor betalingen blev oprettet.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Tidspunkt hvor betalingen blev gennemført (hvis relevant).
        /// </summary>
        public DateTime? PaidAt { get; set; }
    }

}
