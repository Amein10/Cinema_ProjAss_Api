using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Domain.Entities
{
    /// <summary>
    /// Angiver hvilken betalingsmetode der er anvendt.
    /// </summary>
    public enum PaymentMethod
    {
        Unknown = 0,
        Card = 1,
        MobilePay = 2,
        Cash = 3
    }

}
