using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema_ProjAss_Domain.Entities;
using Cinema_ProjAss_Domain.Interfaces;
using Cinema_ProjAss_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cinema_ProjAss_Infrastructure.Repositories
{
    /// <summary>
    /// EF Core implementation af IMovieRepository.
    /// Indeholder CRUD og søgning på titel samt eager loading af genrer.
    /// </summary>
    public class MovieRepository : IMovieRepository
    {
        private readonly CinemaDbContext _context;

        /// <summary>
        /// Opretter repository med DbContext via dependency injection.
        /// </summary>
        public MovieRepository(CinemaDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _context.Movies
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Movie?> GetByIdAsync(int id)
        {
            return await _context.Movies
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Movie>> SearchByTitleAsync(string title)
        {
            return await _context.Movies
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .Where(m => m.Title.Contains(title))
                .ToListAsync();
        }


        /// <inheritdoc />
        public async Task<Movie> AddAsync(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return movie;
        }

        /// <inheritdoc />
        public async Task UpdateAsync(Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return;

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }
    }
}

