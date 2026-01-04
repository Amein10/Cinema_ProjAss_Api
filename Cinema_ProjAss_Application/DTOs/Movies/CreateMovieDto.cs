using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Application.DTOs.Movies
{
    public class CreateMovieDto
    {
        public string Title { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }

        // Genre navne, fx ["Action", "Comedy"]
        public List<string> Genres { get; set; } = new();
    }
}
