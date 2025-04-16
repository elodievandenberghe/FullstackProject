using BookingSite.Domains.Models;
using BookingSite.Repositories;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services.Interfaces;

namespace BookingSite.Services;

public class MealchoicesService : IMealChoiceService
{
    private IMealChoiceDAO _mealChoiceDAO;

    public MealchoicesService(IMealChoiceDAO mealchoiceDAO) 
    {
        _mealChoiceDAO = mealchoiceDAO;
    }

    public async Task AddAsync(MealChoice entity)
    {
        await _mealChoiceDAO.AddAsync(entity);
    }

    public async Task DeleteAsync(MealChoice entity)
    {
        await _mealChoiceDAO.DeleteAsync(entity);
    }

    public async Task<MealChoice?> FindByIdAsync(int Id)
    {
        return await _mealChoiceDAO.FindByIdAsync(Id);
    }

    public async Task<IEnumerable<MealChoice>?> GetAllAsync()
    {
        return await _mealChoiceDAO.GetAllAsync();
    }

    public async Task UpdateAsync(MealChoice entity)
    {
        await _mealChoiceDAO.UpdateAsync(entity);
    }

    public async Task<IEnumerable<MealChoice>?> GetByAirportId(int airportId)
    {
       return  await _mealChoiceDAO.GetByAirportId(airportId);
    }
}