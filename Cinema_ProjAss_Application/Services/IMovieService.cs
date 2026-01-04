using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema_ProjAss_Application.DTOs.Movies;

namespace Cinema_ProjAss_Application.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> GetAllAsync();
        Task<MovieDto> GetByIdAsync(int id);
        Task<IEnumerable<MovieDto>> SearchByTitleAsync(string title);
        Task<MovieDto> CreateAsync(CreateMovieDto dto);
        Task UpdateAsync(int id, CreateMovieDto dto);
        Task DeleteAsync(int id);
    }
}

