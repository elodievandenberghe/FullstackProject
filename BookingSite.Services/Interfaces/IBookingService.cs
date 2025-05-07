using BookingSite.Domains.Models;

namespace BookingSite.Services.Interfaces;

public interface IBookingService : IService<Booking, int>
{
    Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(string userId);
}
