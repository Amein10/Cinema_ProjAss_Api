using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema_ProjAss_Application.DTOs.Shows;

namespace Cinema_ProjAss_Application.Services
{
    public interface IShowService
    {
        Task<ShowDto> GetByIdAsync(int id);
        Task<IEnumerable<ShowDto>> GetShowsByMovieAsync(int movieId);
        Task<IEnumerable<ShowDto>> GetUpcomingShowsAsync(DateTime fromDate);

        Task<ShowDto> CreateAsync(CreateShowDto dto);
        Task UpdateAsync(int id, CreateShowDto dto);
        Task DeleteAsync(int id);
    }
}
