using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Domain.Entities
{
    /// <summary>
    /// Junction-entity der forbinder Movie og Genre.
    /// Bruges til at modellere mange-til-mange-relationen.
    /// </summary>
    public class MovieGenre
    {
        /// <summary>
        /// Fremmednøgle til Movie.
        /// </summary>
        public int MovieId { get; set; }

        /// <summary>
        /// Navigation til film.
        /// </summary>
        public Movie Movie { get; set; } = null!;

        /// <summary>
        /// Fremmednøgle til Genre.
        /// </summary>
        public int GenreId { get; set; }

        /// <summary>
        /// Navigation til genre.
        /// </summary>
        public Genre Genre { get; set; } = null!;
    }

}
