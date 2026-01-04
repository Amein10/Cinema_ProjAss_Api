using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Domain.Entities
{
    /// <summary>
    /// Repræsenterer en filmgenre, fx Action, Drama eller Comedy.
    /// </summary>
    public class Genre
    {
        /// <summary>
        /// Primær nøgle for genren.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Navn på genren.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Relation til film via junction-entity MovieGenre.
        /// </summary>
        public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
    }

}
