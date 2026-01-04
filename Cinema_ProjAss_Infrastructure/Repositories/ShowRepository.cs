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
    /// EF Core implementation af IShowRepository.
    /// Håndterer persistence for visninger (shows) inkl. eager loading af Movie og Auditorium.
    /// </summary>
    public class ShowRepository : IShowRepository
    {
        private readonly CinemaDbContext _context;

        /// <summary>
        /// Opretter repository med DbContext via dependency injection.
        /// </summary>
        public ShowRepository(CinemaDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<Show?> GetByIdAsync(int id)
        {
            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.Auditorium)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Show>> GetShowsByMovieAsync(int movieId)
        {
            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.Auditorium)
                .Where(s => s.MovieId == movieId)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Show>> GetUpcomingShowsAsync(DateTime fromDate)
        {
            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.Auditorium)
                .Where(s => s.StartTime >= fromDate)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Show> AddAsync(Show show)
        {
            _context.Shows.Add(show);
            await _context.SaveChangesAsync();
            return show;
        }

        /// <inheritdoc />
        public async Task UpdateAsync(Show show)
        {
            _context.Shows.Update(show);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int id)
        {
            var show = await _context.Shows.FindAsync(id);
            if (show == null) return;

            _context.Shows.Remove(show);
            await _context.SaveChangesAsync();
        }
    }
}

