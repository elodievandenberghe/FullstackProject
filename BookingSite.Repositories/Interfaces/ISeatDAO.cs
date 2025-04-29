using BookingSite.Domains.Models;

namespace BookingSite.Repositories.Interfaces;

public interface ISeatDAO: IDAO<Seat, int>
{
    Task<IEnumerable<Seat>?> GetByClassId(int id);
}