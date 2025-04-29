using BookingSite.Domains.Models;

namespace BookingSite.Services.Interfaces;

public interface ISeatService: IService<Seat, int>
{
    Task<IEnumerable<Seat>?> GetByClassId(int id);
}