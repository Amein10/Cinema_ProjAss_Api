using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema_ProjAss_Domain.Entities;

namespace Cinema_ProjAss_Domain.Interfaces
{
    /// <summary>
    /// Repository-kontrakt for håndtering af genrer.
    /// Bruges bl.a. når film oprettes/opdateres med genre-navne.
    /// </summary>
    public interface IGenreRepository
    {
        /// <summary>
        /// Finder en genre ud fra navn (case-insensitive anbefales i implementation).
        /// Returnerer null hvis den ikke findes.
        /// </summary>
        Task<Genre?> GetByNameAsync(string name);

        /// <summary>
        /// Opretter en ny genre.
        /// </summary>
        Task<Genre> AddAsync(Genre genre);
    }
}
