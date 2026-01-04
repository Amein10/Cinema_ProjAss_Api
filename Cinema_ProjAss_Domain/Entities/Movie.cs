using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Domain.Entities
{
    /// <summary>
    /// Repræsenterer en film der kan vises i biografen.
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Primær nøgle for filmen.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Filmens titel.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Filmens varighed i minutter.
        /// </summary>
        public int DurationMinutes { get; set; }

        /// <summary>
        /// Filmens genrer via mange-til-mange-relation.
        /// </summary>
        public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();

        /// <summary>
        /// Alle visninger af denne film.
        /// </summary>
        public ICollection<Show> Shows { get; set; } = new List<Show>();
    }

}
