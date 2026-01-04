using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema_ProjAss_Domain.Entities;

namespace Cinema_ProjAss_Domain.Interfaces
{
    /// <summary>
    /// Repository-kontrakt for håndtering af visninger (Show).
    /// Indeholder metoder til at hente visninger pr. film, kommende visninger,
    /// samt at hente/oprette/opdatere/slette en visning.
    /// </summary>
    public interface IShowRepository
    {
        /// <summary>
        /// Henter alle visninger for en given film.
        /// </summary>
        Task<IEnumerable<Show>> GetShowsByMovieAsync(int movieId);

        /// <summary>
        /// Henter alle kommende visninger fra og med en given dato/tid.
        /// </summary>
        Task<IEnumerable<Show>> GetUpcomingShowsAsync(DateTime fromDate);

        /// <summary>
        /// Henter en visning ud fra dens id. Returnerer null hvis den ikke findes.
        /// </summary>
        Task<Show?> GetByIdAsync(int id);

        /// <summary>
        /// Opretter en ny visning.
        /// </summary>
        Task<Show> AddAsync(Show show);

        /// <summary>
        /// Opdaterer en eksisterende visning.
        /// </summary>
        Task UpdateAsync(Show show);

        /// <summary>
        /// Sletter en visning ud fra dens id.
        /// </summary>
        Task DeleteAsync(int id);
    }
}

