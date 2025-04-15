using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services.Interfaces;

namespace BookingSite.Services;

public class SeatService : IService<Seat, int>
{
    private IDAO<Seat, int> _seatDAO;

    public SeatService(IDAO<Seat, int> seatDao) 
    {
        _seatDAO = seatDao;
    }

    public async Task AddAsync(Seat entity)
    {
        await _seatDAO.AddAsync(entity);
    }

    public async Task DeleteAsync(Seat entity)
    {
        await _seatDAO.DeleteAsync(entity);
    }

    public async Task<Seat?> FindByIdAsync(int Id)
    {
        return await _seatDAO.FindByIdAsync(Id);
    }

    public async Task<IEnumerable<Seat>?> GetAllAsync()
    {
        return await _seatDAO.GetAllAsync();
    }

    public async Task UpdateAsync(Seat entity)
    {
        await _seatDAO.UpdateAsync(entity);
    }
}