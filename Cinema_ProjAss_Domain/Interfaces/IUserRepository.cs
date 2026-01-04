using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema_ProjAss_Domain.Entities;

namespace Cinema_ProjAss_Domain.Interfaces
{
    /// <summary>
    /// Repository-kontrakt for brugerdata (login/register).
    /// </summary>
    public interface IUserRepository
    {
        Task<AppUser?> GetByUsernameAsync(string username);
        Task<AppUser?> GetByIdAsync(int id);
        Task<AppUser> AddAsync(AppUser user);
    }
}

