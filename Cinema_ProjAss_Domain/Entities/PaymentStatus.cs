using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Domain.Entities
{
    /// <summary>
    /// Angiver status for en betaling.
    /// </summary>
    public enum PaymentStatus
    {
        Pending = 0,
        Paid = 1,
        Failed = 2,
        Refunded = 3
    }

}
