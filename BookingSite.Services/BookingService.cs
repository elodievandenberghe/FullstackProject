using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services.Interfaces;

namespace BookingSite.Services;

public class BookingService : IBookingService
{
    private IBookingDAO _bookingDAO;

    public BookingService(IBookingDAO bookingDAO)
    {
        _bookingDAO = bookingDAO;
    }

    public async Task AddAsync(Booking entity)
    {
        await _bookingDAO.AddAsync(entity);
    }

    public async Task DeleteAsync(Booking entity)
    {
        await _bookingDAO.DeleteAsync(entity);
    }

    public async Task<Booking?> FindByIdAsync(int id)
    {
        return await _bookingDAO.FindByIdAsync(id);
    }

    public async Task<IEnumerable<Booking>?> GetAllAsync()
    {
        return await _bookingDAO.GetAllAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(string userId)
    {
        return await _bookingDAO.GetBookingsByUserIdAsync(userId);
    }

    public async Task UpdateAsync(Booking entity)
    {
        await _bookingDAO.UpdateAsync(entity);
    }
    public async Task<IEnumerable<String>?> GetCityLattitudeLongitudeOfLastBookedTicketsAsync(string userId)
    {
        return await _bookingDAO.GetCityLattitudeLongitudeOfLastBookedTicketsAsync(userId);
    }
}
