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
    public class SeatRepository : ISeatRepository
    {
        private readonly CinemaDbContext _db;

        public SeatRepository(CinemaDbContext db) => _db = db;

        public Task<List<Seat>> GetByAuditoriumAsync(int auditoriumId)
        {
            return _db.Seats
                .AsNoTracking()
                .Where(s => s.AuditoriumId == auditoriumId)
                .OrderBy(s => s.Row)
                .ThenBy(s => s.Number)
                .ToListAsync();
        }

        public Task<List<Seat>> GetByIdsAsync(IEnumerable<int> seatIds)
        {
            var ids = seatIds.Distinct().ToList();

            return _db.Seats
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();
        }
    }
}
