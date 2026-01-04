using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Application.DTOs.Shows
{
    /// <summary>
    /// Output-model for en visning (show).
    /// Indeholder både id'er og "visnings-venlige" navne (MovieTitle, AuditoriumName).
    /// </summary>
    public class ShowDto
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public decimal Price { get; set; }

        public int MovieId { get; set; }
        public string MovieTitle { get; set; } = string.Empty;

        public int AuditoriumId { get; set; }
        public string AuditoriumName { get; set; } = string.Empty;
    }
}


