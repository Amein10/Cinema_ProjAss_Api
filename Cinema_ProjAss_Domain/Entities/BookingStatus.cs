using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Domain.Entities
{
    /// <summary>
    /// Angiver den aktuelle status for en booking.
    /// </summary>
    public enum BookingStatus
    {
        /// <summary>
        /// Bookingen er oprettet, men endnu ikke bekræftet.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// Bookingen er bekræftet og gyldig.
        /// </summary>
        Confirmed = 1,

        /// <summary>
        /// Bookingen er annulleret.
        /// </summary>
        Cancelled = 2
    }

}
