using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services.Interfaces;

namespace BookingSite.Services;

public class SeasonsService : IService<Season, int>
{
    private IDAO<Season, int> _seasonDAO;

    public SeasonsService(IDAO<Season, int> seasonDAO) 
    {
        _seasonDAO = seasonDAO;
    }

    public async Task AddAsync(Season entity)
    {
        await _seasonDAO.AddAsync(entity);
    }

    public async Task DeleteAsync(Season entity)
    {
        await _seasonDAO.DeleteAsync(entity);
    }

    public async Task<Season?> FindByIdAsync(int Id)
    {
        return await _seasonDAO.FindByIdAsync(Id);
    }

    public async Task<IEnumerable<Season>?> GetAllAsync()
    {
        return await _seasonDAO.GetAllAsync();
    }

    public async Task UpdateAsync(Season entity)
    {
        await _seasonDAO.UpdateAsync(entity);
    }
}