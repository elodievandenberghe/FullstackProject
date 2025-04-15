using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services.Interfaces;

namespace BookingSite.Services;

public class TravelClassesService : IService<TravelClass, int>
{
    private IDAO<TravelClass, int> _travelClassDAO;

    public TravelClassesService(IDAO<TravelClass, int> travelDao) 
    {
        _travelClassDAO = travelDao;
    }

    public async Task AddAsync(TravelClass entity)
    {
        await _travelClassDAO.AddAsync(entity);
    }

    public async Task DeleteAsync(TravelClass entity)
    {
        await _travelClassDAO.DeleteAsync(entity);
    }

    public async Task<TravelClass?> FindByIdAsync(int Id)
    {
        return await _travelClassDAO.FindByIdAsync(Id);
    }

    public async Task<IEnumerable<TravelClass>?> GetAllAsync()
    {
        return await _travelClassDAO.GetAllAsync();
    }

    public async Task UpdateAsync(TravelClass entity)
    {
        await _travelClassDAO.UpdateAsync(entity);
    }
}