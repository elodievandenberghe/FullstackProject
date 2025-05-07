using BookingSite.Domains.Models;

namespace BookingSite.Repositories.Interfaces;

public interface IBookingDAO : IDAO<Booking, int>
{
    Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(string userId);
}
