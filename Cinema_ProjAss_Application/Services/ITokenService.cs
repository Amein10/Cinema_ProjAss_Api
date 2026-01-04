using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema_ProjAss_Domain.Entities;

namespace Cinema_ProjAss_Application.Services
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
