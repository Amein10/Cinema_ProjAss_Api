using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema_ProjAss_Domain.Entities;

namespace Cinema_ProjAss_Domain.Interfaces
{
    public interface ISeatRepository
    {
        Task<List<Seat>> GetByAuditoriumAsync(int auditoriumId);
        Task<List<Seat>> GetByIdsAsync(IEnumerable<int> seatIds);
    }
}
