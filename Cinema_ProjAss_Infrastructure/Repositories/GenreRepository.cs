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
    /// EF Core implementation af IGenreRepository.
    /// </summary>
    public class GenreRepository : IGenreRepository
    {
        private readonly CinemaDbContext _context;

        public GenreRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<Genre?> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            var trimmed = name.Trim();

            // Case-insensitive match. (SQL Server collation er ofte case-insensitive,
            // men vi gør det eksplicit for klarhed)
            return await _context.Genres
                .FirstOrDefaultAsync(g => g.Name.ToLower() == trimmed.ToLower());
        }

        public async Task<Genre> AddAsync(Genre genre)
        {
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            return genre;
        }
    }
}
