using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema_ProjAss_Application.DTOs.Movies;
using Cinema_ProjAss_Application.Exceptions;
using Cinema_ProjAss_Domain.Entities;
using Cinema_ProjAss_Domain.Interfaces;

namespace Cinema_ProjAss_Application.Services
{
    /// <summary>
    /// Application service for film: validerer input, håndterer genrer og mapper til/fra DTOs.
    /// </summary>
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movies;
        private readonly IGenreRepository _genres;

        public MovieService(IMovieRepository movies, IGenreRepository genres)
        {
            _movies = movies;
            _genres = genres;
        }

        public async Task<IEnumerable<MovieDto>> GetAllAsync()
        {
            var list = await _movies.GetAllAsync();
            return list.Select(MapToDto);
        }

        public async Task<MovieDto> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ValidationException("id must be > 0.");

            var movie = await _movies.GetByIdAsync(id);
            if (movie == null) throw new NotFoundException($"Movie with id={id} not found.");

            return MapToDto(movie);
        }

        public async Task<IEnumerable<MovieDto>> SearchByTitleAsync(string title)
        {
            title = title?.Trim() ?? string.Empty;
            if (title.Length == 0) return Array.Empty<MovieDto>();

            var list = await _movies.SearchByTitleAsync(title);
            return list.Select(MapToDto);
        }

        public async Task<MovieDto> CreateAsync(CreateMovieDto dto)
        {
            Validate(dto);

            var movie = new Movie
            {
                Title = dto.Title.Trim(),
                DurationMinutes = dto.DurationMinutes
            };

            // 1) Gem Movie først (så vi får Id)
            var created = await _movies.AddAsync(movie);

            // 2) Tilknyt genrer (opretter Genre + MovieGenre i memory)
            await ApplyGenresAsync(created, dto.Genres);

            // ✅ 3) VIGTIGT: Gem MovieGenres til DB
            await _movies.UpdateAsync(created);

            // 4) Hent igen med includes (MovieGenres->Genre)
            var loaded = await _movies.GetByIdAsync(created.Id);
            if (loaded == null) throw new NotFoundException("Movie was created but could not be loaded.");

            return MapToDto(loaded);
        }


        public async Task UpdateAsync(int id, CreateMovieDto dto)
        {
            if (id <= 0) throw new ValidationException("id must be > 0.");
            Validate(dto);

            var movie = await _movies.GetByIdAsync(id);
            if (movie == null) throw new NotFoundException($"Movie with id={id} not found.");

            movie.Title = dto.Title.Trim();
            movie.DurationMinutes = dto.DurationMinutes;

            // Opdater genre-links
            await ApplyGenresAsync(movie, dto.Genres);

            await _movies.UpdateAsync(movie);

            // (Valgfrit) Du kan returnere en DTO her, men dit interface siger void for UpdateAsync.
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0) throw new ValidationException("id must be > 0.");

            var movie = await _movies.GetByIdAsync(id);
            if (movie == null) throw new NotFoundException($"Movie with id={id} not found.");

            await _movies.DeleteAsync(id);
        }

        private static void Validate(CreateMovieDto dto)
        {
            if (dto == null) throw new ValidationException("Body is required.");
            if (string.IsNullOrWhiteSpace(dto.Title)) throw new ValidationException("Title is required.");
            if (dto.DurationMinutes <= 0) throw new ValidationException("DurationMinutes must be > 0.");
        }

        /// <summary>
        /// Normaliserer, opretter manglende genrer og synkroniserer MovieGenres (replace-all).
        /// </summary>
        private async Task ApplyGenresAsync(Movie movie, List<string> genreNames)
        {
            // Normaliser input (trim + fjern tomme + distinct case-insensitive)
            var normalized = (genreNames ?? new List<string>())
                .Select(g => (g ?? string.Empty).Trim())
                .Where(g => g.Length > 0)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            // Hvis ingen genrer ønskes, ryd koblinger
            if (normalized.Count == 0)
            {
                movie.MovieGenres.Clear();
                return;
            }

            // Find eller opret Genre entities
            var genreEntities = new List<Genre>();

            foreach (var name in normalized)
            {
                var existing = await _genres.GetByNameAsync(name);
                if (existing != null)
                {
                    genreEntities.Add(existing);
                    continue;
                }

                // Opret ny genre
                var created = await _genres.AddAsync(new Genre { Name = name });
                genreEntities.Add(created);
            }

            // Synkroniser MovieGenres:
            // Replace-all approach (nemmest/robust til projekt)
            movie.MovieGenres.Clear();

            foreach (var g in genreEntities)
            {
                movie.MovieGenres.Add(new MovieGenre
                {
                    MovieId = movie.Id,
                    GenreId = g.Id,
                    Movie = movie,
                    Genre = g
                });
            }
        }

        private static MovieDto MapToDto(Movie m)
        {
            return new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                DurationMinutes = m.DurationMinutes,
                Genres = m.MovieGenres.Select(mg => mg.Genre.Name).ToList()
            };
        }
    }
}
