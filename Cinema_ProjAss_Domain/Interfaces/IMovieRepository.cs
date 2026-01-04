using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema_ProjAss_Domain.Entities;

namespace Cinema_ProjAss_Domain.Interfaces
{
    /// <summary>
    /// Repository-kontrakt for håndtering af film (Movie).
    /// Indeholder CRUD-operationer samt søgning på titel.
    /// </summary>
    public interface IMovieRepository
    {
        /// <summary>
        /// Henter alle film i systemet.
        /// </summary>
        /// <returns>Liste af film.</returns>
        Task<IEnumerable<Movie>> GetAllAsync();

        /// <summary>
        /// Henter en film ud fra dens id.
        /// Returnerer null hvis filmen ikke findes.
        /// </summary>
        /// <param name="id">Filmens id.</param>
        /// <returns>Movie eller null.</returns>
        Task<Movie?> GetByIdAsync(int id);

        /// <summary>
        /// Søger efter film baseret på titel (fx delvis match).
        /// Bruges typisk til søgefunktion i klienten.
        /// </summary>
        /// <param name="title">Titel eller del af titel.</param>
        /// <returns>Film der matcher søgningen.</returns>
        Task<IEnumerable<Movie>> SearchByTitleAsync(string title);

        /// <summary>
        /// Tilføjer en ny film til systemet.
        /// </summary>
        /// <param name="movie">Film-objektet der skal oprettes.</param>
        /// <returns>Den oprettede film (typisk inkl. genereret Id).</returns>
        Task<Movie> AddAsync(Movie movie);

        /// <summary>
        /// Opdaterer en eksisterende film.
        /// Forventer at filmen allerede eksisterer (ellers håndteres det i repository-implementering).
        /// </summary>
        /// <param name="movie">Film-objektet med opdaterede værdier.</param>
        Task UpdateAsync(Movie movie);

        /// <summary>
        /// Sletter en film ud fra dens id.
        /// Bruges typisk af admin-funktionalitet.
        /// </summary>
        /// <param name="id">Id på filmen der skal slettes.</param>
        Task DeleteAsync(int id);
    }
}

