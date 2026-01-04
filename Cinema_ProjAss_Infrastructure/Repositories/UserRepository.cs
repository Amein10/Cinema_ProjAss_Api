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
    /// EF Core implementation af IUserRepository.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly CinemaDbContext _context;

        public UserRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser?> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return null;

            var u = username.Trim();
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == u);
        }

        public async Task<AppUser?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> AddAsync(AppUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}

