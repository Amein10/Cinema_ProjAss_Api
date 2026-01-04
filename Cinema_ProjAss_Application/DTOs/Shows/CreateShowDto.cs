using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Application.DTOs.Shows
{
    /// <summary>
    /// Input-model til at oprette en ny visning (Show).
    /// </summary>
    public class CreateShowDto
    {
        /// <summary>
        /// Starttidspunkt for visningen.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Pris pr. billet for visningen.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Id på filmen der skal vises.
        /// </summary>
        public int MovieId { get; set; }

        /// <summary>
        /// Id på salen hvor visningen foregår.
        /// </summary>
        public int AuditoriumId { get; set; }
    }
}
