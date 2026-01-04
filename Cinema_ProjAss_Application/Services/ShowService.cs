using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema_ProjAss_Application.DTOs.Shows;
using Cinema_ProjAss_Application.Exceptions;
using Cinema_ProjAss_Domain.Entities;
using Cinema_ProjAss_Domain.Interfaces;

namespace Cinema_ProjAss_Application.Services
{
    /// <summary>
    /// Application service for visninger (shows).
    /// Validerer input, opretter shows og mapper entities til DTOs.
    /// </summary>
    public class ShowService : IShowService
    {
        private readonly IShowRepository _shows;
        private readonly IMovieRepository _movies;

        /// <summary>
        /// Opretter ShowService med nødvendige repositories via DI.
        /// </summary>
        public ShowService(IShowRepository shows, IMovieRepository movies)
        {
            _shows = shows;
            _movies = movies;
        }

        public async Task<ShowDto> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ValidationException("id must be > 0.");

            var show = await _shows.GetByIdAsync(id);
            if (show == null) throw new NotFoundException($"Show with id={id} not found.");

            return MapToDto(show);
        }

        public async Task<IEnumerable<ShowDto>> GetShowsByMovieAsync(int movieId)
        {
            if (movieId <= 0) throw new ValidationException("movieId must be > 0.");

            var list = await _shows.GetShowsByMovieAsync(movieId);
            return list.Select(MapToDto);
        }

        public async Task<IEnumerable<ShowDto>> GetUpcomingShowsAsync(DateTime fromDate)
        {
            if (fromDate == default) throw new ValidationException("fromDate is required.");

            var list = await _shows.GetUpcomingShowsAsync(fromDate);
            return list.Select(MapToDto);
        }

        public async Task<ShowDto> CreateAsync(CreateShowDto dto)
        {
            Validate(dto);

            // Valider at film findes
            var movie = await _movies.GetByIdAsync(dto.MovieId);
            if (movie == null) throw new ValidationException($"MovieId={dto.MovieId} does not exist.");

            // Auditorium validering kræver et AuditoriumRepository.
            // Hvis du ikke har det endnu, lader vi DB/EF håndhæve FK og håndterer fejl i API-laget.

            var show = new Show
            {
                StartTime = dto.StartTime,
                Price = dto.Price,
                MovieId = dto.MovieId,
                AuditoriumId = dto.AuditoriumId
            };

            var created = await _shows.AddAsync(show);

            // Hent igen for at sikre navigation properties (Movie/Auditorium) er loaded
            var loaded = await _shows.GetByIdAsync(created.Id);
            if (loaded == null) throw new NotFoundException("Show was created but could not be loaded.");

            return MapToDto(loaded);
        }

        private static void Validate(CreateShowDto dto)
        {
            if (dto == null) throw new ValidationException("Body is required.");

            if (dto.MovieId <= 0) throw new ValidationException("MovieId must be > 0.");
            if (dto.AuditoriumId <= 0) throw new ValidationException("AuditoriumId must be > 0.");

            // Pris: vælg hvad du ønsker:
            // Hvis gratis visninger skal være tilladt: brug dto.Price < 0
            // Hvis pris altid skal være positiv: brug dto.Price <= 0
            if (dto.Price < 0) throw new ValidationException("Price must be >= 0.");

            if (dto.StartTime == default) throw new ValidationException("StartTime is required.");

            // Valgfrit: kræv at starttidspunkt ligger i fremtiden
            if (dto.StartTime <= DateTime.UtcNow)
                throw new ValidationException("StartTime must be in the future (UTC).");
        }

        private static ShowDto MapToDto(Show s)
        {
            return new ShowDto
            {
                Id = s.Id,
                StartTime = s.StartTime,
                Price = s.Price,
                MovieId = s.MovieId,
                MovieTitle = s.Movie?.Title ?? string.Empty,
                AuditoriumId = s.AuditoriumId,
                AuditoriumName = s.Auditorium?.Name ?? string.Empty
            };
        }
    }
}

