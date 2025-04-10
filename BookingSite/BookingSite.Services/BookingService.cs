using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services.Interfaces;

namespace BookingSite.Services;

public class BookingService : IService<Booking, int>
{
    private IDAO<Booking, int> _bookingDAO;

    public BookingService(IDAO<Booking, int> bookingDAO) // DI
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

    public async Task<Booking?> FindByIdAsync(int Id)
    {
        return await _bookingDAO.FindByIdAsync(Id);
    }

    public async Task<IEnumerable<Booking>?> GetAllAsync()
    {
        return await _bookingDAO.GetAllAsync();
    }

    public async Task UpdateAsync(Booking entity)
    {
        await _bookingDAO.UpdateAsync(entity);
    }
}