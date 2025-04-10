using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services.Interfaces;

namespace BookingSite.Services;

public class CityService : IService<City, int>
{
    private IDAO<City, int> _cityDAO;

    public CityService(IDAO<City, int> cityDao) // DI
    {
        _cityDAO = cityDao;
    }

    public async Task AddAsync(City entity)
    {
        await _cityDAO.AddAsync(entity);
    }

    public async Task DeleteAsync(City entity)
    {
        await _cityDAO.DeleteAsync(entity);
    }

    public async Task<City?> FindByIdAsync(int Id)
    {
        return await _cityDAO.FindByIdAsync(Id);
    }

    public async Task<IEnumerable<City>?> GetAllAsync()
    {
        return await _cityDAO.GetAllAsync();
    }

    public async Task UpdateAsync(City entity)
    {
        await _cityDAO.UpdateAsync(entity);
    }
}