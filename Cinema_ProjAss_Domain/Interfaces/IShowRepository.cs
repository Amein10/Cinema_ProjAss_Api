using Cinema_ProjAss_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_ProjAss_Domain.Interfaces
{
    public interface IShowRepository
    {
        Task<IEnumerable<Show>> GetShowsByMovieAsync(int movieId);
        Task<IEnumerable<Show>> GetUpcomingShowsAsync(DateTime fromDate);

        Task<Show?> GetByIdAsync(int id);
        Task<Show> AddAsync(Show show);
    }
}
