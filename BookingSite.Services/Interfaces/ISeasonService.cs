using BookingSite.Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSite.Services.Interfaces
{
    public interface ISeasonService : IService<Season, int>
    {
        Task<IEnumerable<Season>?> GetByAirportId(int airportId);
    }
}
